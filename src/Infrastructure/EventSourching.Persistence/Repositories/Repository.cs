using EventSourching.Application.Abstractions;
using EventSourching.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventSourching.Persistence.Repositories
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        readonly SourchingDbContext _context;
        public Repository(SourchingDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table { get => _context.Set<T>(); }

        public async Task AddAsync(T model)
              => await Table.AddAsync(model);

        public async Task DeleteAsync(T model)
            => _context.Remove(model);

        public IQueryable<T> GetAll()
            => Table;

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method)
            => Table.Where(method);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
