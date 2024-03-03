using EventSourching.Domain.Aggregates.UserAggregate.ValueObjects;
using EventSourching.Domain.Common.Abstractions;

namespace EventSourching.Domain.Aggregates.UserAggregate
{
    public class User : AggregateRoot<UserId>
    {
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }

        public User()
        {

        }

        public User(Guid id, string username, string email, string password) : base(UserId.Create(id))
        {
            Id = UserId.Create(id);
            Username = username;
            Email = email;
            Password = password;
            CreatedDate = DateTime.Now;
        }

        public User(UserId id, string username, string email, string password) : base(id)
        {
            Username = username;
            Email = email;
            Password = password;
            CreatedDate = DateTime.Now;
        }


        public static User Create(string username, string email, string password)
            => new(UserId.CreateUnique(), username, email, password);

        public static User Create(Guid Id, string username, string email, string password)
            => new(Id, username, email, password);

        public static User Create(UserId Id, string username, string email, string password)
            => new(Id, username, email, password);

        public void AddUserDomainEvent(IDomainEvent @event, Type type)
        {
            AddDomainEvent(@event, type);
        }

        public Type GetUserDomainEventType(string evnetName)
            => GetTypeForDic(evnetName);
    }
}
