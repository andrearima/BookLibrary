
namespace BookLibrary.Royal.Infrastructure;

public interface IQueryExecutor
{
    Task<IEnumerable<T>> Execute<T>(string sql, object parameters = null);
}