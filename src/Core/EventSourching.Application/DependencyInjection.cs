using EventSourching.Domain.Aggregates.UserAggregate.ValueObjects;
using EventSourching.Domain.Aggregates.UserAggregate;
using Microsoft.Extensions.DependencyInjection;
using EventSourching.Application.Repositories;
using EventSourching.Domain.Common.Abstractions;
using EventStore.ClientAPI;
using MediatR;
using EventSourching.Application.Abstractions;

namespace EventSourching.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection ApplicationServiceRegistration(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            var connection = serviceProvider.GetRequiredService<IEventStoreConnection>();

            var triggeredEventRepository = serviceProvider.GetRequiredService<ITriggeredEventRepository>();

            services.AddAutoMapper(AssemblyReference.Assembly);

            services.AddMediatR(AssemblyReference.Assembly);

            services.AddSingleton<IDomainEventRepository<User, UserId>>(sp =>
            {
                return new DomainEventRepository<User, UserId>(connection, serviceProvider, triggeredEventRepository);
            });

            return services;
        }
    }
}
