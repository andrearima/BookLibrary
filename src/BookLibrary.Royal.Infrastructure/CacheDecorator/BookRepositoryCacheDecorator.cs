using BookLibrary.Royal.Domain.Entities;
using BookLibrary.Royal.Domain.Filter;
using BookLibrary.Royal.Domain.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BookLibrary.Royal.Infrastructure.CacheDecorator;

public class BookRepositoryCacheDecorator : CacheDecoratorBase, IBookRepository
{
    private readonly IBookRepository _bookRepository;
    private readonly ILogger<BookRepositoryCacheDecorator> _logger;
    public BookRepositoryCacheDecorator(IBookRepository bookRepository, ILogger<BookRepositoryCacheDecorator> logger, IMemoryCache memoryCache, IOptions<CacheConfiguration> options) 
        : base(memoryCache, logger, options)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Book>> GetBooks(BookFilter filter)
    {
        _logger.LogDebug("Checking if Book exists in cache");

        return await TryGetFromCache($"Book_{filter.GetKey()}", async () =>
        {
            _logger.LogDebug("Dictionary Language [Code: languageCode] not found in cache, trying to get from DB");

            return await _bookRepository.GetBooks(filter);
        });
    }
}
