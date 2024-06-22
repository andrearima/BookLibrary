using BookLibrary.Royal.Domain.Entities;
using BookLibrary.Royal.Domain.Filter;

namespace BookLibrary.Royal.Domain.Repository;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetBooks(BookFilter filter);
}
