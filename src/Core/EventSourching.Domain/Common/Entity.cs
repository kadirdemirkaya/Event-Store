using EventSourching.Domain.Common.Abstractions;
using System.Linq;


namespace EventSourching.Domain.Common
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>, IHasDomainEvents
     where TId : notnull
    {
        public TId Id { get; protected set; }
        public DateTime CreatedDate { get; protected set; }


        protected List<IDomainEvent> _domainEvents = new();
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();


        protected Dictionary<string, Type> _eventTypes = new();

        public Entity()
        {
        }
        protected Entity(TId id)
        {
            Id = id;
        }

        public override bool Equals(object? obj)
        {
            return obj is Entity<TId> Entity && Id.Equals(Entity.Id);
        }

        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Entity<TId>? other)
        {
            return Equals((object?)other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected void AddDomainEvent(IDomainEvent domainEvent, Type type)
        {
            _domainEvents.Add(domainEvent);
            if (!_eventTypes.ContainsKey(type.ToString()))
                _eventTypes.Add(TypeTrin(type), type);
        }

        public string TypeTrin(Type type)
        {
            int lastDotIndex = type.ToString().LastIndexOf('.');
            string trimmedTypeName = type.ToString().Substring(lastDotIndex + 1);
            trimmedTypeName = trimmedTypeName.ToLower();
            return trimmedTypeName;
        }

        public Type GetTypeForDic(string eventType)
        {
            if (_eventTypes.ContainsKey(eventType.Trim('"').ToLower()))
                return _eventTypes[eventType.Trim('"').ToLower()];
            return default;
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public void ClearEventTypes()
        {
            _eventTypes.Clear();
        }
    }
}
