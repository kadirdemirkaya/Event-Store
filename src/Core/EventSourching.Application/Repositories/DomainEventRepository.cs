using EventSourching.Application.Abstractions;
using EventSourching.Domain.Common;
using EventSourching.Domain.Common.Abstractions;
using EventStore.ClientAPI;
using System.Text;
using System.Text.Json;

namespace EventSourching.Application.Repositories
{
    public class DomainEventRepository<T, TId> : IDomainEventRepository<T, TId>
        where T : Entity<TId>
        where TId : ValueObject
    {
        private readonly IEventStoreConnection _connection;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITriggeredEventRepository _triggeredEventRepository;
        public DomainEventRepository(IEventStoreConnection connection, IServiceProvider serviceProvider, ITriggeredEventRepository triggeredEventRepository)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _triggeredEventRepository = triggeredEventRepository;
        }

        public async Task<bool> SendAsync(T aggregate)
        {
            try
            {
                List<EventData> events = aggregate.DomainEvents
                .Select(@event => new EventData(
                    eventId: @event.Id,
                    type: @event.GetType().Name,
                    isJson: true,
                    data: Encoding.UTF8.GetBytes(@event.Payload),
                    metadata: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event.Type))))
                .ToList();

                if (!events.Any())
                    return default;

                await _connection.AppendToStreamAsync(GetStreamName(), ExpectedVersion.Any, events);
                await GetDataAsync(GetStreamName(), aggregate);
                aggregate.ClearDomainEvents();
                aggregate.ClearEventTypes();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Events>> GetEvents(string streamName, T aggregate)
        {
            try
            {
                long nextSliceStart = 0L;
                List<ResolvedEvent> events = new();
                StreamEventsSlice readEvents = null;
                do
                {
                    readEvents = await _connection.ReadStreamEventsForwardAsync(
                        stream: streamName,
                        start: nextSliceStart,
                        count: 4096,
                        resolveLinkTos: true
                    );

                    if (readEvents.Events.Length > 0)
                        events.AddRange(readEvents.Events);

                    nextSliceStart = readEvents.NextEventNumber;
                } while (!readEvents.IsEndOfStream);

                return events.Select(@event => new Events
                {
                    EventNumber = @event.Event.EventNumber,
                    EventType = @event.Event.EventType,
                    Created = @event.Event.Created,
                    EventId = @event.Event.EventId,
                    EventStreamId = @event.Event.EventStreamId,
                    Metadata = Encoding.UTF8.GetString(@event.Event.Metadata),
                    Data = Encoding.UTF8.GetString(@event.Event.Data)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteStreamAsync()
        {
            await _connection.DeleteStreamAsync(GetStreamName(), ExpectedVersion.Any);
        }

        public async Task GetDataAsync(string streamName, T aggregate)
        {
            var events = await GetEvents(streamName, aggregate);
            try
            {
                foreach (var @event in events)
                {
                    var DATAS = _triggeredEventRepository.GetAll().ToList();
                    if (_triggeredEventRepository.GetWhere(t => t.TriggerId == @event.EventId).Any())
                        continue;
                    var type = aggregate.GetTypeForDic(@event.Metadata);
                    var data = JsonSerializer.Deserialize(@event.Data.ToString(), type);
                    await PublishEvents(type, data);
                    await _triggeredEventRepository.AddAsync(new() { TriggerId = @event.EventId });
                    await _triggeredEventRepository.SaveChangesAsync();
                }
                #region test
                // it is not right to delete
                //await DeleteStreamAsync();

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task PublishEvents(Type typeName, object data)
        {
            try
            {
                var domainEventHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(typeName);
                var domainEventHandler = _serviceProvider.GetService(domainEventHandlerType);
                if (domainEventHandler == null)
                    return;
                await (Task)domainEventHandlerType.GetMethod("Handle").Invoke(domainEventHandler, new object[] { data });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GetStreamName()
        {
            DateTime now = DateTime.Now;
            string year = now.Year.ToString();
            string month = now.Month.ToString().PadLeft(2, '0');
            string day = now.Day.ToString().PadLeft(2, '0');

            return $"{year}{month}{day}";
        }

        private Type GetTypeWithMetaDate(byte[] metadata, T aggregate)

            => aggregate.GetTypeForDic(Encoding.UTF8.GetString(metadata));
    }
}
