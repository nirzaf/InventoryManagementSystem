using System.Net;
using FluentAssertions;

namespace InventoryManagementSystem.Tests.Integration;

public class StockControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public StockControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task Get_StockIndex_RedirectsToLogin_WhenUnauthenticated()
    {
        var response = await _client.GetAsync("/Stock/Index");
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        response.Headers.Location?.OriginalString.Should().Contain("Account/Login");
    }

    [Fact]
    public async Task Get_StockReceive_RedirectsToLogin_WhenUnauthenticated()
    {
        var response = await _client.GetAsync("/Stock/Receive");
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Get_StockTransfer_RedirectsToLogin_WhenUnauthenticated()
    {
        var response = await _client.GetAsync("/Stock/Transfer");
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Get_StockSell_RedirectsToLogin_WhenUnauthenticated()
    {
        var response = await _client.GetAsync("/Stock/Sell");
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Get_StockTransactions_RedirectsToLogin_WhenUnauthenticated()
    {
        var response = await _client.GetAsync("/Stock/Transactions");
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Post_StockReceive_WithoutAuth_RedirectsToLogin()
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["itemId"] = "1",
            ["locationId"] = "1",
            ["quantity"] = "10",
            ["notes"] = "test"
        });
        var response = await _client.PostAsync("/Stock/Receive", content);
        response.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }
}
