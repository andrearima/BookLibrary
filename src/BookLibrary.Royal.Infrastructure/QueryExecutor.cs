using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BookLibrary.Royal.Infrastructure;

public class QueryExecutor : IQueryExecutor
{
    private readonly IDbConnection _connection;
    private readonly ILogger<QueryExecutor> _logger;

    public QueryExecutor(IDbConnection connection, ILogger<QueryExecutor> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task<IEnumerable<T>> Execute<T>(string sql, object parameters = null)
    {
        try
        {
            _logger.LogInformation("Executing query: {sql}", sql);

            var result = await _connection.QueryAsync<T>(sql, parameters);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing query: {sql}", sql);

            throw;
        }
    }
}
