using EventSourching.Domain.Aggregates.UserAggregate;

namespace EventSourching.Application.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {

    }
}
