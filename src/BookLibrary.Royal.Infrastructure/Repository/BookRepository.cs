using BookLibrary.Royal.Domain.Entities;
using BookLibrary.Royal.Domain.Filter;
using BookLibrary.Royal.Domain.Repository;
using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace BookLibrary.Royal.Infrastructure.Repository;

public class BookRepository : IBookRepository
{
    private readonly IQueryExecutor _queryExecutor;
    private readonly ObjectPool<StringBuilder> _stringBuilderPool;
    public BookRepository(IQueryExecutor queryExecutor, ObjectPool<StringBuilder> stringBuilderPool)
    {
        _queryExecutor = queryExecutor;
        _stringBuilderPool = stringBuilderPool;
    }

    public Task<IEnumerable<Book>> GetBooks(BookFilter filter)
    {
        const string query = @"SELECT
	                                book_id as Id,
	                                title as Title,
	                                first_name as FirstName,
	                                last_name as LastName,
	                                total_copies as TotalCopies,
	                                copies_in_use as CopiesInUse,
	                                [type] as Type,
	                                isbn as Isbn,
	                                category as Category
                                FROM
	                                books WITH (NOLOCK) ";

        var queryFilter = _stringBuilderPool.Get();

        var queryWithWhere = query + filter.BuildWhereClause(queryFilter);

        try
        {
            return _queryExecutor.Execute<Book>(queryWithWhere);
        }
        finally
        {
            _stringBuilderPool.Return(queryFilter);
        }
    }
}
