using EventSourching.Domain.Aggregates.UserAggregate;
using EventSourching.Domain.Common.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EventSourching.Persistence.Data
{
    public class SourchingDbContext : DbContext
    {
        public SourchingDbContext(DbContextOptions options) : base(options)
        {
        }

        public SourchingDbContext()
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("SourchingMemoryDb");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<List<IDomainEvent>>();
            modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
