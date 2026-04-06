# Worker Service

Monitor background workers and hosted services:

```csharp
using ServiceMonitor.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHubMon(options =>
{
    options.DashboardUrl = "https://api.hubmon.com";
    options.ApiKey = "sm_live_...";
    options.ServiceName = "email-worker";
    options.Environment = "production";
    options.Version = "1.0.0";
    options.EnableMetrics = true;
    options.HeartbeatInterval = TimeSpan.FromMinutes(1);
});

builder.Services.AddHostedService<EmailWorker>();

var host = builder.Build();
host.Run();

class EmailWorker : BackgroundService
{
    private readonly ILogger<EmailWorker> _logger;

    public EmailWorker(ILogger<EmailWorker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Processing email queue...");
            // Process emails
            await Task.Delay(10000, stoppingToken);
        }
    }
}
```

::: tip
Worker Services don't have an HTTP pipeline, so `EnableRequestTracking` and `UseHubMonTracking()` are not applicable.
:::
