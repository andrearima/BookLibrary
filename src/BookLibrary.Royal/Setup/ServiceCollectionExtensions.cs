using BookLibrary.Royal.App;
using BookLibrary.Royal.Domain.Repository;
using BookLibrary.Royal.Infrastructure;
using BookLibrary.Royal.Infrastructure.CacheDecorator;
using BookLibrary.Royal.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.ObjectPool;
using System.Data;
using System.Text;

namespace BookLibrary.Royal.Setup;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheConfiguration>(configuration.GetSection("Cache"));

        services.AddScoped<IBookApp, BookApp>();
        services.AddScoped<IQueryExecutor, QueryExecutor>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.Decorate<IBookRepository, BookRepositoryCacheDecorator>();
        services.AddMemoryCache();

        services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
        services.AddSingleton<ObjectPool<StringBuilder>>(serviceProvider =>
        {
            var provider = serviceProvider.GetRequiredService<ObjectPoolProvider>();
            var policy = new StringBuilderPooledObjectPolicy();
            return provider.Create(policy);
        });

        services.AddScoped<IDbConnection>(provider =>
        {
            var connectionString = configuration.GetConnectionString("Book");

            var connection = new SqlConnection(connectionString);
            var logger = provider.GetService<ILogger<SqlConnection>>();

            if (logger != null && logger.IsEnabled(LogLevel.Debug))
            {
                connection.StateChange += (sender, e) =>
                {
                    logger.LogDebug("SqlConnection Changed Status. [CurrentState: {CurrentState}; OriginalState: {OriginalState}; ConnectionId: {WorkstationId} {ClientConnectionId}]", e?.CurrentState, e?.OriginalState, connection?.WorkstationId, connection?.ClientConnectionId);
                };

                connection.InfoMessage += (sender, e) =>
                {
                    logger.LogDebug("SqlConnection InfoMessage. [Message: {Message}; ConnectionId: {WorkstationId} {ClientConnectionId}]", e?.Message, connection?.WorkstationId, connection?.ClientConnectionId);
                };
            }

            return connection;
        });
        return services;
    }
}
