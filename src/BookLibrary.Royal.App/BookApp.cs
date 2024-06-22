using BookLibrary.Royal.Domain.Entities;
using BookLibrary.Royal.Domain.Filter;
using BookLibrary.Royal.Domain.Repository;

namespace BookLibrary.Royal.App;

public class BookApp : IBookApp
{
    private readonly IBookRepository _bookRepository;

    public BookApp(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Task<IEnumerable<Book>> GetBooks(BookFilter filter)
    {
        return _bookRepository.GetBooks(filter);
    }
}
