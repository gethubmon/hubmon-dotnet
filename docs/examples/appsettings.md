# Using appsettings.json

Bind configuration from `appsettings.json` instead of hardcoding values:

## appsettings.json

```json
{
  "HubMon": {
    "DashboardUrl": "https://api.hubmon.com",
    "ApiKey": "sm_live_your_api_key",
    "ServiceName": "my-service",
    "Environment": "production",
    "Version": "1.0.0",
    "EnableMetrics": true,
    "EnableRequestTracking": true,
    "HeartbeatInterval": "00:00:30",
    "RetryAttempts": 3
  }
}
```

## Program.cs

```csharp
builder.Services.AddHubMon(options =>
{
    builder.Configuration.GetSection("HubMon").Bind(options);
});
```

## Environment-Specific Config

**appsettings.Development.json:**
```json
{
  "HubMon": {
    "DashboardUrl": "http://localhost:5192",
    "Environment": "development",
    "HeartbeatInterval": "00:00:10"
  }
}
```

**appsettings.Production.json:**
```json
{
  "HubMon": {
    "DashboardUrl": "https://api.hubmon.com",
    "Environment": "production",
    "HeartbeatInterval": "00:00:30"
  }
}
```

## Environment Variables

Override any setting with environment variables using the `__` separator:

```bash
export HubMon__DashboardUrl=https://api.hubmon.com
export HubMon__ApiKey=sm_live_your_key
export HubMon__ServiceName=my-api
```

Or in Docker Compose:

```yaml
environment:
  - HubMon__DashboardUrl=https://api.hubmon.com
  - HubMon__ApiKey=sm_live_your_key
```
