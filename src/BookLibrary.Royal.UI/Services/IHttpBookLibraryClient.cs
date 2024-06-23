using BookLibrary.Royal.UI.Models;

namespace BookLibrary.Royal.UI.Services
{
    public interface IHttpBookLibraryClient
    {
        Task<IEnumerable<Book>?> GetBooksAsync(SearchBook? search = null);
    }
}