using FirstAppForBank.Domain.Enums;
using Xunit;

namespace FirstAppForBank.UnitTests;

public sealed class ServiceStatusTests
{
    [Fact]
    public void Unknown_Should_Not_Be_Ok()
    {
        Assert.NotEqual(ServiceStatus.Ok, ServiceStatus.Unknown);
    }
}
