using EventSourching.Domain.Aggregates.TriggeredEventAggretages;
using EventSourching.Domain.Aggregates.UserAggregate;
using EventSourching.Domain.Common.Abstractions;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<TriggeredEvent> TriggeredEvents { get; set; }

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
