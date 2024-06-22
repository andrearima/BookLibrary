using BookLibrary.Royal.Infrastructure.Messaging;
using BookLibrary.Royal.Integration.Tests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using NSubstitute;
using System.Data;

namespace BookLibrary.Royal.Integration.Tests;

public class BookLibraryWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IDbConnection _dbConnection;
    public readonly IRabbitMqGetBookNotifier RabbitMq = Substitute.For<IRabbitMqGetBookNotifier>();
    public BookLibraryWebApplicationFactory()
    {
        _dbConnection = new SqliteConnection("DataSource=file:mem?mode=memory&cache=shared");
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        builder
            .ConfigureServices(services =>
            {
                services.AddMvc()
                    .AddApplicationPart(typeof(Program).Assembly);

                services.Substitute<IDbConnection>(_dbConnection);
                services.Substitute<IRabbitMqGetBookNotifier>(RabbitMq);
            });
    }
}
