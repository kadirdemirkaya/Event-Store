using EventSourching.Domain.Aggregates.UserAggregate.Events;
using EventSourching.Domain.Common.Abstractions;

namespace EventSourching.Persistence.EventHandlers
{
    public class UserStateUpdateDomainEventHandler : IDomainEventHandler<UserStateUpdateDomainEvent>
    {
        public async Task Handle(UserStateUpdateDomainEvent @event)
        {
            
        }
    }
}
