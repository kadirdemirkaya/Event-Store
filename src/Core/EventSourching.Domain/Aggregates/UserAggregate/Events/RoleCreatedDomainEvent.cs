using EventSourching.Domain.Common.Abstractions;
using EventSourching.Domain.Common;
using System.Text.Json;

namespace EventSourching.Domain.Aggregates.UserAggregate.Events
{
    public class RoleCreatedDomainEvent : DomainEvent, IDomainEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = typeof(RoleCreatedDomainEvent).Name;
        public string Email { get; set; }
        public string Payload { get; set; }
        public bool IsActive { get; set; } = true;

        public RoleCreatedDomainEvent(Guid id,string email)
        {
            Id = id;
            Email = email;
        }

        public void AddSerializeData(RoleCreatedDomainEvent roleCreatedDomainEvent)
        {
            Payload = JsonSerializer.Serialize(roleCreatedDomainEvent);
        }
    }
}
