using BookLibrary.Royal.Domain.Filter;
using NSubstitute;
using System.Net.Http;

namespace BookLibrary.Royal.Integration.Tests;

public class GetBookEndpoint : IClassFixture<BookLibraryWebApplicationFactory>
{
    private readonly BookLibraryWebApplicationFactory _factory;
    private readonly HttpClient _httpClient;
    public GetBookEndpoint(BookLibraryWebApplicationFactory factory)
    {
        _factory = factory;
        _httpClient = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_Should_SendMessage()
    {
        // Arrange & Act
        _ = await _httpClient.GetAsync("/Book");

        // Assert
        _factory.RabbitMq.Received().SendMessage(Arg.Any<BookFilter>());
    }
}