# ServiceMonitorOptions Reference

Complete reference for all `ServiceMonitorOptions` properties.

## Required

```csharp
public string DashboardUrl { get; set; }  // HubMon API URL
public string ApiKey { get; set; }         // API key (sm_live_...)
public string ServiceName { get; set; }    // Service name
```

## Service Identity

```csharp
public string Environment { get; set; } = "production";
public string? Version { get; set; }
public string? Hostname { get; set; }       // Auto: Environment.MachineName
public string? InstanceId { get; set; }     // Auto: hostname:port
public int? Port { get; set; }             // Auto: Kestrel detection
public string? Url { get; set; }           // Auto: Kestrel detection
```

## Monitoring

```csharp
public TimeSpan HeartbeatInterval { get; set; } = TimeSpan.FromSeconds(30);
public bool EnableMetrics { get; set; } = false;
public bool EnableRequestTracking { get; set; } = false;
public bool EnableLogging { get; set; } = true;
public int RetryAttempts { get; set; } = 3;
```

## Deployment Metadata

```csharp
public string? BuildId { get; set; }
public string? ReleaseId { get; set; }
public DateTime? BuildDate { get; set; }
public DateTime? DeploymentDate { get; set; }
public string? CommitHash { get; set; }
public string? Branch { get; set; }
public string? BuildConfiguration { get; set; }
public Dictionary<string, object>? DeploymentMetadata { get; set; }
```
