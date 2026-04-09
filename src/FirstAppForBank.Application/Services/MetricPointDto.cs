namespace FirstAppForBank.Application.Services;

public sealed class MetricPointDto
{
    public DateTimeOffset Timestamp { get; init; }

    public double Value { get; init; }
}
