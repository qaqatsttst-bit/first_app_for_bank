using System.Globalization;
using System.Text.Json;
using FirstAppForBank.Application.Services;
using FirstAppForBank.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace FirstAppForBank.Infrastructure.Services;

public sealed class PrometheusMetricHistoryReader(HttpClient httpClient, IOptions<PrometheusOptions> options) : IMetricHistoryReader
{
    private readonly PrometheusOptions prometheusOptions = options.Value;

    public async Task<ServiceMetricsDto> GetMetricsAsync(string serviceName, MetricsRange range, CancellationToken cancellationToken = default)
    {
        var (start, end, step) = GetRangeParameters(range);

        var latency = await QuerySeriesAsync("latency", "Latency", "ms", prometheusOptions.Queries.Latency, serviceName, start, end, step, cancellationToken);
        var errorRate = await QuerySeriesAsync("error_rate", "Error Rate", "%", prometheusOptions.Queries.ErrorRate, serviceName, start, end, step, cancellationToken);
        var availability = await QuerySeriesAsync("availability", "Availability", "%", prometheusOptions.Queries.Availability, serviceName, start, end, step, cancellationToken);

        return new ServiceMetricsDto
        {
            Range = range,
            Series = [latency, errorRate, availability]
        };
    }

    private async Task<MetricSeriesDto> QuerySeriesAsync(
        string key,
        string displayName,
        string unit,
        string queryTemplate,
        string serviceName,
        DateTimeOffset start,
        DateTimeOffset end,
        string step,
        CancellationToken cancellationToken)
    {
        var query = BuildQuery(queryTemplate, serviceName);
        var url = $"{prometheusOptions.BaseUrl.TrimEnd('/')}/api/v1/query_range?query={Uri.EscapeDataString(query)}&start={Uri.EscapeDataString(start.ToString("O", CultureInfo.InvariantCulture))}&end={Uri.EscapeDataString(end.ToString("O", CultureInfo.InvariantCulture))}&step={Uri.EscapeDataString(step)}";

        try
        {
            using var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            var points = ParsePoints(document.RootElement);
            return new MetricSeriesDto
            {
                Key = key,
                DisplayName = displayName,
                Unit = unit,
                Points = points
            };
        }
        catch
        {
            return new MetricSeriesDto
            {
                Key = key,
                DisplayName = displayName,
                Unit = unit,
                Points = Array.Empty<MetricPointDto>()
            };
        }
    }

    private string BuildQuery(string queryTemplate, string serviceName)
    {
        return queryTemplate
            .Replace("{{service}}", serviceName, StringComparison.Ordinal)
            .Replace("{label}", prometheusOptions.ServiceLabel, StringComparison.Ordinal);
    }

    private static IReadOnlyCollection<MetricPointDto> ParsePoints(JsonElement root)
    {
        if (!root.TryGetProperty("data", out var data) || !data.TryGetProperty("result", out var result) || result.GetArrayLength() == 0)
        {
            return Array.Empty<MetricPointDto>();
        }

        var values = result[0].GetProperty("values");
        var points = new List<MetricPointDto>(values.GetArrayLength());

        foreach (var value in values.EnumerateArray())
        {
            if (value.GetArrayLength() < 2)
            {
                continue;
            }

            var timestamp = value[0].GetDouble();
            var rawValue = value[1].GetString();

            if (!double.TryParse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
            {
                continue;
            }

            points.Add(new MetricPointDto
            {
                Timestamp = DateTimeOffset.FromUnixTimeSeconds((long)timestamp),
                Value = Math.Round(parsedValue, 2)
            });
        }

        return points;
    }

    private static (DateTimeOffset start, DateTimeOffset end, string step) GetRangeParameters(MetricsRange range)
    {
        var end = DateTimeOffset.UtcNow;

        return range switch
        {
            MetricsRange.Last24Hours => (end.AddHours(-24), end, "2h"),
            MetricsRange.Last7Days => (end.AddDays(-7), end, "12h"),
            MetricsRange.Last30Days => (end.AddDays(-30), end, "2d"),
            _ => (end.AddHours(-24), end, "2h")
        };
    }
}
