namespace EventSourching.Domain.Common.Abstractions
{
    public interface IHasDomainEvents
    {
        public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    }
}
