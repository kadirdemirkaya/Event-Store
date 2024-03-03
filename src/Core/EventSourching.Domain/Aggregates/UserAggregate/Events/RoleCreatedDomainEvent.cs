using EventSourching.Domain.Common.Abstractions;
using EventSourching.Domain.Common;
using System.Text.Json;

namespace EventSourching.Domain.Aggregates.UserAggregate.Events
{
    public class RoleCreatedDomainEvent : DomainEvent, IDomainEvent
    {
        public string Type { get; set; } = typeof(RoleCreatedDomainEvent).Name;
        public string Email { get; set; }
        public string Payload { get; set; }

        public RoleCreatedDomainEvent(string email)
        {
            Email = email;
        }

        public void AddSerializeData(RoleCreatedDomainEvent roleCreatedDomainEvent)
        {
            Payload = JsonSerializer.Serialize(roleCreatedDomainEvent);
        }
    }
}
