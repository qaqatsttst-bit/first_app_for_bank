using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FirstAppForBank.IntegrationTests;

public sealed class ServicesEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public ServicesEndpointTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task GetServices_ShouldReturnOk()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/services");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
