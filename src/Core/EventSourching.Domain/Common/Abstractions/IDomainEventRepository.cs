namespace EventSourching.Domain.Common.Abstractions
{
    public interface IDomainEventRepository<T, TId>
        where T : Entity<TId>
        where TId : ValueObject
    {
        Task<bool> SendAsync(T aggregate);
    }
}
