using EventStore.ClientAPI;

namespace EventSourching.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection ApiServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            IEventStoreConnection connection = GetConnection(configuration);

            connection.ConnectAsync().GetAwaiter().GetResult();

            services.AddSingleton(connection);

            return services;
        }

        static IEventStoreConnection GetConnection(IConfiguration configuration)
           => EventStoreConnection.Create(
                       connectionString: $"ConnectTo={configuration["Connection:Url"]};DefaultUserCredentials={configuration["Connection:Credentials"]};UseSslConnection=true;TargetHost={configuration["Connection:Host"]};ValidateServer=false",
                       connectionName: configuration["Connection:Name"],
                       builder: ConnectionSettings.Create().KeepReconnecting()
                   );
    }
}
