using EventSourching.Domain.Aggregates.UserAggregate.Events;
using EventSourching.Domain.Common.Abstractions;

namespace EventSourching.Persistence.EventHandlers
{
    public class UserCreatedDomainEventHandler : IDomainEventHandler<UserCreatedDomainEvent>
    {
        public async Task Handle(UserCreatedDomainEvent @event)
        {

        }
    }
}
