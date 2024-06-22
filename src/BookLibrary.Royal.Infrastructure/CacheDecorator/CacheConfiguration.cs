namespace BookLibrary.Royal.Infrastructure.CacheDecorator;

public class CacheConfiguration
{
    /// <summary>
    /// default timeout in seconds (5 minuts)
    /// Cache:TimeoutSeconds
    /// </summary>
    public int TimeoutSeconds { get; set; } = 300;
}
