using EventSourching.Domain.Aggregates.UserAggregate.Events;
using EventSourching.Domain.Common.Abstractions;

namespace EventSourching.Persistence.EventHandlers
{
    public class RoleCreatedDomainEventHandler : IDomainEventHandler<RoleCreatedDomainEvent>
    {
        public async Task Handle(RoleCreatedDomainEvent @event)
        {

        }
    }
}
