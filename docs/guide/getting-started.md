# Getting Started

## Installation

Install the HubMon SDK via NuGet:

```bash
dotnet add package HubMon.Client --prerelease
```

::: info
The SDK is currently in pre-release (`1.0.0-alpha.1`). Use `--prerelease` to install the latest version. Once `1.0.0` stable is released, you can omit this flag.
:::

## Quick Start

Add HubMon to your ASP.NET Core application:

```csharp
using ServiceMonitor.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceMonitor(options =>
{
    options.DashboardUrl = "https://api.hubmon.com";
    options.ApiKey = "sm_live_your_api_key_here";
    options.ServiceName = "my-api";
    options.Environment = "production";
    options.EnableMetrics = true;
});

var app = builder.Build();
app.Run();
```

That's it! Your service will automatically:

1. **Register** with HubMon on startup
2. **Send heartbeats** every 30 seconds
3. **Report metrics** — CPU, memory, disk, and thread count

## Getting Your API Key

1. Sign up at [HubMon](https://hubmon.com)
2. Navigate to **API Keys** in your dashboard
3. Click **Create New Key**
4. Copy the key (starts with `sm_live_`)

::: warning
The API key is shown only once — save it securely!
:::

## Enable Request Tracking

For web applications, you can also track requests per minute and active connections:

```csharp
builder.Services.AddServiceMonitor(options =>
{
    // ...
    options.EnableRequestTracking = true;
});

var app = builder.Build();
app.UseHubMonTracking();  // Add before other middleware
app.Run();
```

## Next Steps

- [Configuration Reference](/guide/configuration) — All available options
- [Metrics](/guide/metrics) — What metrics are collected
- [Examples](/examples/aspnet-web-api) — Full working examples
