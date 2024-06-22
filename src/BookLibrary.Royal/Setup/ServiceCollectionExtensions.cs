using BookLibrary.Royal.App;
using BookLibrary.Royal.Domain.Repository;
using BookLibrary.Royal.Infrastructure;
using BookLibrary.Royal.Infrastructure.CacheDecorator;
using BookLibrary.Royal.Infrastructure.Messaging;
using BookLibrary.Royal.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.ObjectPool;
using System.Data;
using System.Text;

namespace BookLibrary.Royal.Setup;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddApps()
                       .ConfigureCaching(configuration)
                       .AddInfrastructure(configuration);
    }

    internal static IServiceCollection AddApps(this IServiceCollection services)
    {
        return services.AddScoped<IBookApp, BookApp>();
    }

    internal static IServiceCollection ConfigureCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CacheConfiguration>(configuration.GetSection("Cache"));

        return services.AddMemoryCache();
    }

    internal static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRabbitMq(configuration);

        services.AddScoped<IQueryExecutor, QueryExecutor>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.Decorate<IBookRepository, BookRepositoryCacheDecorator>();

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

            return connection;
        });

        return services;
    }

    internal static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)));

        services.AddSingleton<IRabbitMqGetBookNotifier, RabbitMqGetBookNotifier>();
    }
}
