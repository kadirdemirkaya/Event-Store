using EventSourching.Domain.Aggregates.TriggeredEventAggretages;

namespace EventSourching.Application.Abstractions
{
    public interface ITriggeredEventRepository : IRepository<TriggeredEvent>
    {
    }
}
