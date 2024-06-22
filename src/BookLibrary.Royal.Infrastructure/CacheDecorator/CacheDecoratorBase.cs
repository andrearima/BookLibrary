using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookLibrary.Royal.Infrastructure.CacheDecorator;

public class CacheDecoratorBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly CacheConfiguration _cacheConfiguration;
    private readonly ILogger<CacheDecoratorBase> _logger;

    public CacheDecoratorBase(IMemoryCache memoryCache, ILogger<CacheDecoratorBase> logger, IOptions<CacheConfiguration> options)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _cacheConfiguration = options.Value;
    }

    protected async Task<TResult> TryGetFromCache<TResult>(string cacheKey, Func<Task<TResult>> getWithoutCache)
    {

        if (_memoryCache.TryGetValue(cacheKey, out TResult cacheResult))
        {
            _logger.LogDebug("Cache found. Cache key: {cacheKey}", cacheKey);
            return cacheResult;
        }

        cacheResult = await getWithoutCache();

        _logger.LogDebug("Set value to cache. Key: {cacheKey}. Expiration: {TimeoutSeconds} seconds", cacheKey, _cacheConfiguration.TimeoutSeconds);
        var cacheExpiration = TimeSpan.FromSeconds(_cacheConfiguration.TimeoutSeconds);
        _memoryCache.Set(cacheKey, cacheResult, cacheExpiration);

        return cacheResult;
    }
}
