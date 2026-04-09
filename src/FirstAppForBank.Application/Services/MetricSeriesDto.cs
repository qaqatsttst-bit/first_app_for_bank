namespace FirstAppForBank.Application.Services;

public sealed class MetricSeriesDto
{
    public string Key { get; init; } = string.Empty;

    public string DisplayName { get; init; } = string.Empty;

    public string Unit { get; init; } = string.Empty;

    public IReadOnlyCollection<MetricPointDto> Points { get; init; } = Array.Empty<MetricPointDto>();
}
