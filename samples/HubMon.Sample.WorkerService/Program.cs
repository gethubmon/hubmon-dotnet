using ServiceMonitor.Client;

var builder = Host.CreateApplicationBuilder(args);

// Add HubMon monitoring
builder.Services.AddServiceMonitor(options =>
{
    options.DashboardUrl = builder.Configuration["HubMon:DashboardUrl"] ?? "http://localhost:5192";
    options.ApiKey = builder.Configuration["HubMon:ApiKey"] ?? "sm_live_your_key_here";
    options.ServiceName = "sample-worker";
    options.Environment = builder.Environment.EnvironmentName.ToLower();
    options.Version = "1.0.0";
    options.HeartbeatInterval = TimeSpan.FromSeconds(30);
    options.EnableMetrics = true;
    options.EnableLogging = true;
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();

class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            await Task.Delay(5000, stoppingToken);
        }
    }
}
