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

        public DomainEventRepository(IEventStoreConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
        }

        public async Task<bool> SendAsync(T aggregate)
        {
            try
            {
                List<EventData> events = aggregate.DomainEvents
                .Select(@event => new EventData(
                    eventId: Guid.NewGuid(),
                    type: @event.GetType().Name,
                    isJson: true,
                    data: Encoding.UTF8.GetBytes(@event.Payload),
                    metadata: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event.Type))))
                .ToList();

                if (!events.Any())
                    return default;

                await _connection.AppendToStreamAsync(typeof(T).Name, ExpectedVersion.Any, events);
                await GetDataAsync(typeof(T).Name, aggregate);
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
            await _connection.DeleteStreamAsync(typeof(T).Name, ExpectedVersion.Any);
        }

        public async Task GetDataAsync(string streamName, T aggregate)
        {
            var events = await GetEvents(streamName, aggregate);
            try
            {
                foreach (var @event in events)
                {
                    var type = aggregate.GetTypeForDic(@event.Metadata);
                    var data = JsonSerializer.Deserialize(@event.Data.ToString(), type);
                    await PublishEvents(type, data);
                }
                await DeleteStreamAsync();
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

        private Type GetTypeWithMetaDate(byte[] metadata, T aggregate)

            => aggregate.GetTypeForDic(Encoding.UTF8.GetString(metadata));
    }
}
