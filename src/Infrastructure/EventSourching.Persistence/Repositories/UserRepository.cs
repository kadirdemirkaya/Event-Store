using EventSourching.Application.Abstractions;
using EventSourching.Domain.Aggregates.UserAggregate;
using EventSourching.Persistence.Data;

namespace EventSourching.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SourchingDbContext context) : base(context)
        {
        }
    }
}
