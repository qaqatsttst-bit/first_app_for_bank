using FirstAppForBank.Application.Services;

namespace FirstAppForBank.Infrastructure.Services;

public sealed class InMemoryMetricHistoryReader : IMetricHistoryReader
{
    public Task<ServiceMetricsDto> GetMetricsAsync(string serviceName, MetricsRange range, CancellationToken cancellationToken = default)
    {
        var seed = Math.Abs(serviceName.GetHashCode(StringComparison.Ordinal));
        var (points, step) = range switch
        {
            MetricsRange.Last24Hours => (12, TimeSpan.FromHours(2)),
            MetricsRange.Last7Days => (14, TimeSpan.FromHours(12)),
            MetricsRange.Last30Days => (15, TimeSpan.FromDays(2)),
            _ => (12, TimeSpan.FromHours(2))
        };

        var end = DateTimeOffset.UtcNow;
        var start = end - TimeSpan.FromTicks(step.Ticks * (points - 1));

        var latency = BuildSeries("latency", "Latency", "ms", points, start, step, 80 + seed % 20, 12);
        var errorRate = BuildSeries("error_rate", "Error Rate", "%", points, start, step, 0.8 + (seed % 5) * 0.1, 0.35);
        var availability = BuildSeries("availability", "Availability", "%", points, start, step, 99.6 - (seed % 3) * 0.05, 0.12);

        return Task.FromResult(new ServiceMetricsDto
        {
            Range = range,
            Series = [latency, errorRate, availability]
        });
    }

    private static MetricSeriesDto BuildSeries(string key, string displayName, string unit, int count, DateTimeOffset start, TimeSpan step, double baseline, double amplitude)
    {
        var points = new List<MetricPointDto>(count);

        for (var i = 0; i < count; i++)
        {
            var value = baseline + Math.Sin(i * 0.7) * amplitude + Math.Cos(i * 0.35) * amplitude * 0.45;
            points.Add(new MetricPointDto
            {
                Timestamp = start.AddTicks(step.Ticks * i),
                Value = Math.Round(value, 2)
            });
        }

        return new MetricSeriesDto
        {
            Key = key,
            DisplayName = displayName,
            Unit = unit,
            Points = points
        };
    }
}
