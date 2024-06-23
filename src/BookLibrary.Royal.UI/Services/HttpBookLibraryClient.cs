using BookLibrary.Royal.UI.Models;
using System.Net.Http.Json;

namespace BookLibrary.Royal.UI.Services;

public class HttpBookLibraryClient : IHttpBookLibraryClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HttpBookLibraryClient> _logger;

    public HttpBookLibraryClient(HttpClient httpClient, ILogger<HttpBookLibraryClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<Book>?> GetBooksAsync(SearchBook? search = null)
    {
        // Should be configurable

        const string Url = "https://localhost:7242/Book";
        try
        {
            var query = search?.ToQueryString();
            return await _httpClient.GetFromJsonAsync<IEnumerable<Book>>($"{Url}?{query}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching books from the server.");
            return new List<Book>();
        }
    }
}
