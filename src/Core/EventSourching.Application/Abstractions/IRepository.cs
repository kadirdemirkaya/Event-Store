using System.Linq.Expressions;

namespace EventSourching.Application.Abstractions
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method);
        Task AddAsync(T model);
        Task DeleteAsync(T model);
        Task SaveChangesAsync();
    }
}
