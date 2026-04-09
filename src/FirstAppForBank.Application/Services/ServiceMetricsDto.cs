namespace FirstAppForBank.Application.Services;

public sealed class ServiceMetricsDto
{
    public MetricsRange Range { get; init; }

    public IReadOnlyCollection<MetricSeriesDto> Series { get; init; } = Array.Empty<MetricSeriesDto>();
}
