using FirstAppForBank.Domain.Enums;

namespace FirstAppForBank.Application.Services;

public sealed class ChangeServiceStatusRequest
{
    public ServiceStatus NewStatus { get; init; }

    public string? Comment { get; init; }

    public string ChangeSource { get; init; } = "UI";

    public string ChangeSourceType { get; init; } = "manual";
}
