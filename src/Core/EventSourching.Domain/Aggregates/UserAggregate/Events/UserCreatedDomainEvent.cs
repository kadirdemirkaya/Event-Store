using EventSourching.Domain.Common.Abstractions;
using EventSourching.Domain.Common;
using System.Text.Json;

namespace EventSourching.Domain.Aggregates.UserAggregate.Events
{
    public class UserCreatedDomainEvent : DomainEvent, IDomainEvent
    {
        public string Type { get; set; } = typeof(UserCreatedDomainEvent).Name;
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Payload { get; set; }

        public UserCreatedDomainEvent(string userName, string email, string password)
        {
            Username = userName;
            Email = email;
            Password = password;
        }

        public void AddSerializeData(UserCreatedDomainEvent userCreatedDomainEvent)
        {
            Payload = JsonSerializer.Serialize(userCreatedDomainEvent);
        }
    }
}
