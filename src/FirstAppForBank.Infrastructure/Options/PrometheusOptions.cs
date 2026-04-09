namespace FirstAppForBank.Infrastructure.Options;

public sealed class PrometheusOptions
{
    public bool Enabled { get; set; }

    public string BaseUrl { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 10;

    public string ServiceLabel { get; set; } = "service";

    public PrometheusQueryTemplates Queries { get; set; } = new();
}

public sealed class PrometheusQueryTemplates
{
    public string Latency { get; set; } = "service_latency_ms{{{label}=\"{{service}}\"}}";

    public string ErrorRate { get; set; } = "service_error_rate{{{label}=\"{{service}}\"}}";

    public string Availability { get; set; } = "service_availability{{{label}=\"{{service}}\"}}";
}
