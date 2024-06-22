using BookLibrary.Royal.Domain.Entities;
using BookLibrary.Royal.Domain.Filter;
using BookLibrary.Royal.Domain.Repository;
using BookLibrary.Royal.Infrastructure.Messaging;

namespace BookLibrary.Royal.App;

public class BookApp : IBookApp
{
    private readonly IBookRepository _bookRepository;
    private readonly IRabbitMqGetBookNotifier _notifier;
    public BookApp(IBookRepository bookRepository, IRabbitMqGetBookNotifier notifier)
    {
        _bookRepository = bookRepository;
        _notifier = notifier;
    }

    public Task<IEnumerable<Book>> GetBooks(BookFilter filter)
    {
        _notifier.SendMessage(filter);
        return _bookRepository.GetBooks(filter);
    }
}
