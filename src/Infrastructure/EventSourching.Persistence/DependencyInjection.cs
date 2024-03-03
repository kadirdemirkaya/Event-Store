using EventSourching.Application.Abstractions;
using EventSourching.Application.Repositories;
using EventSourching.Domain.Aggregates.UserAggregate;
using EventSourching.Domain.Aggregates.UserAggregate.Events;
using EventSourching.Domain.Aggregates.UserAggregate.ValueObjects;
using EventSourching.Domain.Common.Abstractions;
using EventSourching.Persistence.Data;
using EventSourching.Persistence.EventHandlers;
using EventSourching.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourching.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection PersistenceServiceRegistration(this IServiceCollection services)
        {
            services.AddDbContext<SourchingDbContext>(options =>
                options.UseInMemoryDatabase("SourchingMemoryDb"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ITriggeredEventRepository, TriggeredEventRepository>();

            services.AddSingleton<IDomainEventHandler<UserCreatedDomainEvent>, UserCreatedDomainEventHandler>();

            services.AddSingleton<IDomainEventHandler<UserStateUpdateDomainEvent>, UserStateUpdateDomainEventHandler>();

            services.AddSingleton<IDomainEventHandler<RoleCreatedDomainEvent>, RoleCreatedDomainEventHandler>();

            return services;
        }
    }
}
