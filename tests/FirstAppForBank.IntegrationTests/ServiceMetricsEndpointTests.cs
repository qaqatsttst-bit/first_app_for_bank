using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FirstAppForBank.IntegrationTests;

public sealed class ServiceMetricsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory;

    public ServiceMetricsEndpointTests(WebApplicationFactory<Program> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task GetMetrics_ShouldReturnOk()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/services/11111111-aaaa-bbbb-cccc-111111111111/metrics?range=24h");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
