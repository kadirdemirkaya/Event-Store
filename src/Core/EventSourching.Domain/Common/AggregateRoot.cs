namespace EventSourching.Domain.Common.Abstractions
{
    public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
        where TId : notnull
    {
        public AggregateRoot()
        {

        }
        protected AggregateRoot(TId id) : base(id)
        {
        }
    }
}
