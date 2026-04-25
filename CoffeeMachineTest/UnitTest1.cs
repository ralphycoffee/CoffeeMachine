using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace CoffeeMachineApi.Tests;

public class BrewCoffeeTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BrewCoffeeTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task BrewCoffee_ReturnsOk_WithMessage()
    {
        var response = await _client.GetAsync("/brew-coffee");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<CoffeeResponse>();

        Assert.NotNull(body);
        Assert.Equal("Your piping hot coffee is ready", body!.Message);
        Assert.False(string.IsNullOrWhiteSpace(body.Prepared));
    }

    [Fact]
    public async Task BrewCoffee_EveryFifthCall_ReturnsServiceUnavailable()
    {
        var responses = new List<HttpResponseMessage>();

        for (int i = 1; i <= 5; i++)
        {
            responses.Add(await _client.GetAsync("/brew-coffee"));
        }

        Assert.Contains(responses, r => r.StatusCode == HttpStatusCode.ServiceUnavailable);
    }

    private class CoffeeResponse
    {
        public string Message { get; set; } = "";
        public string Prepared { get; set; } = "";
    }
}