using BookLibrary.Royal.Domain.Entities;
using BookLibrary.Royal.Domain.Filter;

namespace BookLibrary.Royal.App;

public interface IBookApp
{
    Task<IEnumerable<Book>> GetBooks(BookFilter filter);
}
