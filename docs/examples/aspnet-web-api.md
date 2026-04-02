# ASP.NET Core Web API

Full example with all features enabled:

```csharp
using ServiceMonitor.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceMonitor(options =>
{
    options.DashboardUrl = builder.Configuration["HubMon:DashboardUrl"]
        ?? "https://api.hubmon.com";
    options.ApiKey = builder.Configuration["HubMon:ApiKey"]
        ?? throw new Exception("HubMon API key is required");
    options.ServiceName = "payment-api";
    options.Environment = builder.Environment.EnvironmentName.ToLower();
    options.Version = "1.0.0";

    // Metrics
    options.EnableMetrics = true;
    options.EnableRequestTracking = true;
    options.HeartbeatInterval = TimeSpan.FromSeconds(15);

    // CI/CD metadata
    options.CommitHash = Environment.GetEnvironmentVariable("GITHUB_SHA");
    options.Branch = Environment.GetEnvironmentVariable("GITHUB_REF_NAME");
    options.DeploymentDate = DateTime.UtcNow;
});

var app = builder.Build();

// Enable request tracking (must be before other middleware)
app.UseHubMonTracking();

app.MapGet("/", () => "Payment API is running!");
app.MapGet("/api/payments", () => new[] { "payment-1", "payment-2" });

app.Run();
```

## appsettings.json

```json
{
  "HubMon": {
    "DashboardUrl": "https://api.hubmon.com",
    "ApiKey": "sm_live_your_api_key_here"
  }
}
```

## Docker Environment

```yaml
services:
  payment-api:
    image: my-payment-api:latest
    environment:
      - HubMon__DashboardUrl=https://api.hubmon.com
      - HubMon__ApiKey=sm_live_your_api_key
```
