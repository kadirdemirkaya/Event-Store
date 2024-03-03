using EventSourching.Domain.Common;
using EventSourching.Domain.Common.Abstractions;
using System.Text.Json;

namespace EventSourching.Domain.Aggregates.UserAggregate.Events
{
    public class UserStateUpdateDomainEvent : DomainEvent, IDomainEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Type { get; set; } = typeof(UserStateUpdateDomainEvent).Name;
        public string Payload { get; set; }

        public Guid UserId { get; set; }
        public string Email { get; set; }

        public bool IsActive { get; set; } = true;
        public UserStateUpdateDomainEvent(Guid id, Guid userId, string email)
        {
            Id = id;
            UserId = userId;
            Email = email;
        }

        public void AddSerializeData(UserStateUpdateDomainEvent userStateUpdateDomainEvent)
        {
            Payload = JsonSerializer.Serialize(userStateUpdateDomainEvent);
        }
    }
}
