namespace FirstAppForBank.Application.Services;

public interface IMetricHistoryReader
{
    Task<ServiceMetricsDto> GetMetricsAsync(string serviceName, MetricsRange range, CancellationToken cancellationToken = default);
}
