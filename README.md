# HubMon.Client

[![NuGet](https://img.shields.io/nuget/v/HubMon.Client.svg)](https://www.nuget.org/packages/HubMon.Client)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HubMon.Client.svg)](https://www.nuget.org/packages/HubMon.Client)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

Official .NET client SDK for the [HubMon](https://hubmon.com) service monitoring platform. Automatically registers your service and sends periodic heartbeats with system metrics.

> **Current version:** `1.1.0` — [View on NuGet](https://www.nuget.org/packages/HubMon.Client)

## Features

- **Easy Integration** — Add monitoring with just 3 lines of code
- **System Metrics** — CPU, memory, disk usage, thread count
- **Request Tracking** — Requests/min and active connections via middleware
- **Auto-Retry** — Built-in retry logic with exponential backoff
- **Deployment Metadata** — Track builds, commits, and releases
- **Lightweight** — Minimal overhead on your application

## Installation

```bash
dotnet add package HubMon.Client
```

## Quick Start

```csharp
using ServiceMonitor.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHubMon(options =>
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
1. Register with HubMon on startup
2. Send heartbeats every 30 seconds
3. Report CPU, memory, disk, and thread metrics

## Request Tracking

Track HTTP requests per minute and active connections:

```csharp
builder.Services.AddHubMon(options =>
{
    options.DashboardUrl = "https://api.hubmon.com";
    options.ApiKey = "sm_live_...";
    options.ServiceName = "my-api";
    options.EnableMetrics = true;
    options.EnableRequestTracking = true;
});

var app = builder.Build();
app.UseHubMonTracking();  // Add tracking middleware
app.Run();
```

## Configuration

```csharp
builder.Services.AddHubMon(options =>
{
    // Required
    options.DashboardUrl = "https://api.hubmon.com";
    options.ApiKey = "sm_live_...";
    options.ServiceName = "my-service";

    // Optional
    options.Environment = "production";             // Default: "production"
    options.Version = "1.0.0";
    options.HeartbeatInterval = TimeSpan.FromSeconds(30);  // Default: 30s, min: 5s
    options.EnableMetrics = true;                   // CPU/Memory/Disk (default: false)
    options.EnableRequestTracking = true;           // RPM/Connections (default: false)
    options.EnableLogging = true;                   // Default: true
    options.RetryAttempts = 3;                      // Default: 3, range: 0-10

    // Deployment metadata (CI/CD)
    options.CommitHash = "a1b2c3d";
    options.Branch = "main";
    options.BuildConfiguration = "Release";
});
```

## Using appsettings.json

```json
{
  "HubMon": {
    "DashboardUrl": "https://api.hubmon.com",
    "ApiKey": "sm_live_your_api_key",
    "ServiceName": "my-service",
    "Environment": "production",
    "EnableMetrics": true,
    "EnableRequestTracking": true
  }
}
```

```csharp
builder.Services.AddHubMon(options =>
{
    builder.Configuration.GetSection("HubMon").Bind(options);
});
```

## Metrics

When `EnableMetrics = true`:

| Metric | Description |
|---|---|
| CPU Usage | Process CPU percentage |
| Memory | Working set in MB |
| Total Memory | Available system memory in MB |
| Disk Usage | Root drive usage percentage |
| Thread Count | Active thread count |

When `EnableRequestTracking = true`:

| Metric | Description |
|---|---|
| Requests/Min | HTTP requests per minute |
| Active Connections | Current concurrent connections |

## Worker Service

```csharp
using ServiceMonitor.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHubMon(options =>
{
    options.DashboardUrl = "https://api.hubmon.com";
    options.ApiKey = "sm_live_...";
    options.ServiceName = "background-worker";
    options.EnableMetrics = true;
    options.HeartbeatInterval = TimeSpan.FromMinutes(1);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
```

## Requirements

- .NET 8.0 or higher
- HubMon account with API key

## Documentation

Full documentation: [https://gethubmon.github.io/hubmon-dotnet](https://gethubmon.github.io/hubmon-dotnet)

## License

MIT License — see [LICENSE](LICENSE) for details.
