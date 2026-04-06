# Request / Response Models

## ServiceRegistrationRequest

Sent to `POST /api/services/register`:

```csharp
class ServiceRegistrationRequest
{
    string ServiceName { get; set; }
    string Environment { get; set; }
    string? Version { get; set; }
    string? InstanceId { get; set; }
    string? Hostname { get; set; }
    int? Port { get; set; }
    string? Url { get; set; }
    int? ProcessId { get; set; }
    Dictionary<string, object>? Metadata { get; set; }
}
```

## ServiceRegistrationResponse

Returned from `POST /api/services/register`:

```csharp
class ServiceRegistrationResponse
{
    Guid ServiceId { get; set; }
    Guid InstanceId { get; set; }
    string ServiceName { get; set; }
    string Environment { get; set; }
    string Status { get; set; }
    string Message { get; set; }
}
```

## HeartbeatRequest

Sent to `POST /api/services/heartbeat`:

```csharp
class HeartbeatRequest
{
    Guid InstanceId { get; set; }
    Dictionary<string, object>? Metadata { get; set; }
}
```

## Extension Methods

### `AddHubMon()`

```csharp
public static IServiceCollection AddHubMon(
    this IServiceCollection services,
    Action<ServiceMonitorOptions> configure)
```

Registers all HubMon services in the DI container.

### `UseHubMonTracking()`

```csharp
public static IApplicationBuilder UseHubMonTracking(
    this IApplicationBuilder app)
```

Adds the request tracking middleware to the pipeline. Required when `EnableRequestTracking = true`.
