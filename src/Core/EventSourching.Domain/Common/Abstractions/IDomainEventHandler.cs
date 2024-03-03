namespace EventSourching.Domain.Common.Abstractions
{
    public interface IDomainEventHandler<IEvent> : IEventHandler where IEvent : DomainEvent
    {
        Task Handle(IEvent @event);
    }

    public interface IEventHandler
    {

    }
}
