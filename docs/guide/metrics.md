# Metrics

## System Metrics

Enable with `EnableMetrics = true`:

| Metric | Key | Type | Description |
|---|---|---|---|
| CPU Usage | `cpu_percent` | `double` | Process CPU usage as percentage |
| Memory | `memory_mb` | `double` | Process working set in MB |
| Total Memory | `total_memory_mb` | `double` | Total available system memory in MB |
| Disk Usage | `disk_usage_percent` | `double` | Root drive usage percentage |
| Thread Count | `thread_count` | `int` | Number of active threads |

### How CPU is Calculated

```
CPU % = (TotalProcessorTime / Uptime) * 100 / ProcessorCount
```

This gives the average CPU usage of the process since it started.

### How Memory is Measured

- **Memory (MB)**: `Process.WorkingSet64` — physical memory used by the process
- **Total Memory (MB)**: `GC.GetGCMemoryInfo().TotalAvailableMemoryBytes` — total system memory

### Disk Usage

Measured from the root drive where the application runs (`AppContext.BaseDirectory`).

## Request Tracking

Enable with `EnableRequestTracking = true` and add the middleware:

```csharp
app.UseHubMonTracking();
```

| Metric | Key | Type | Description |
|---|---|---|---|
| Requests/Min | `requests_per_minute` | `long` | HTTP requests in the last minute |
| Active Connections | `active_connections` | `long` | Current concurrent HTTP connections |

::: tip
Request tracking requires the ASP.NET Core middleware pipeline. It is not available in Worker Services.
:::

## Viewing Metrics

All metrics are visible in the HubMon dashboard under each service instance. Historical data is retained based on your subscription plan.
