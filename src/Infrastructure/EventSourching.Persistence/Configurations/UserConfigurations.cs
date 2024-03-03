using EventSourching.Domain.Aggregates.UserAggregate;
using EventSourching.Domain.Aggregates.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventSourching.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(l => l.Id);

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Id,
                   value => UserId.Create(value));

            builder.Property(l => l.CreatedDate);

            builder.Property(l => l.Email);

            builder.Property(l => l.Password);

            builder.Property(l => l.Username);
        }
    }
}
