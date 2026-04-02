# Configuration

All options are set via `AddServiceMonitor()`:

```csharp
builder.Services.AddServiceMonitor(options =>
{
    // Configure here
});
```

## Required Options

| Option | Type | Description |
|---|---|---|
| `DashboardUrl` | `string` | HubMon API URL (e.g., `https://api.hubmon.com`) |
| `ApiKey` | `string` | Your API key (format: `sm_live_...`) |
| `ServiceName` | `string` | Name of the service (e.g., `"payment-api"`) |

## Optional Options

| Option | Type | Default | Description |
|---|---|---|---|
| `Environment` | `string` | `"production"` | Environment name (dev, staging, production) |
| `Version` | `string?` | `null` | Service version |
| `Hostname` | `string?` | Auto-detected | Machine hostname |
| `InstanceId` | `string?` | Auto-generated | Unique instance identifier |
| `Port` | `int?` | Auto-detected | Listening port |
| `Url` | `string?` | Auto-detected | Full service URL |
| `HeartbeatInterval` | `TimeSpan` | `30s` | Heartbeat frequency (min: 5s) |
| `EnableMetrics` | `bool` | `false` | Collect CPU/Memory/Disk metrics |
| `EnableRequestTracking` | `bool` | `false` | Track RPM and active connections |
| `EnableLogging` | `bool` | `true` | Enable SDK logging |
| `RetryAttempts` | `int` | `3` | Retry count for failed requests (0-10) |

## Deployment Metadata

Track CI/CD information with each registration:

| Option | Type | Description |
|---|---|---|
| `BuildId` | `string?` | CI build ID (e.g., Azure DevOps) |
| `ReleaseId` | `string?` | Release ID |
| `BuildDate` | `DateTime?` | When the build was created |
| `DeploymentDate` | `DateTime?` | When this instance was deployed |
| `CommitHash` | `string?` | Git commit hash |
| `Branch` | `string?` | Git branch name |
| `BuildConfiguration` | `string?` | Build configuration (Release/Debug) |
| `DeploymentMetadata` | `Dictionary<string, object>?` | Custom key-value metadata |

## Validation Rules

The SDK validates configuration at startup:

- `DashboardUrl` must be a valid absolute URL
- `ApiKey` cannot be empty
- `ServiceName` cannot be empty
- `HeartbeatInterval` must be at least 5 seconds
- `RetryAttempts` must be between 0 and 10

Invalid configuration throws `ArgumentException` at startup.
