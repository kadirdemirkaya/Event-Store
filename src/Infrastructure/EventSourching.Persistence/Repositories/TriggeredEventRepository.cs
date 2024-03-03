using EventSourching.Application.Abstractions;
using EventSourching.Domain.Aggregates.TriggeredEventAggretages;
using EventSourching.Persistence.Data;

namespace EventSourching.Persistence.Repositories
{
    public class TriggeredEventRepository : Repository<TriggeredEvent>, ITriggeredEventRepository
    {
        public TriggeredEventRepository(SourchingDbContext context) : base(context)
        {
        }
    }
}
