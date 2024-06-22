using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BookLibrary.Royal.Integration.Tests.Extensions;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection Substitute<TInterface>(this IServiceCollection services, TInterface value)
    {
        if (value == null) throw new ArgumentException(nameof(value));

        services.Replace(new ServiceDescriptor(typeof(TInterface), value));

        return services;
    }
}
