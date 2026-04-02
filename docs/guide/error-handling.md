# Error Handling

The SDK is designed to fail gracefully — monitoring issues should never crash your application.

## Registration Failure

If service registration fails (network error, invalid API key, etc.):

- The error is logged (if `EnableLogging = true`)
- Your application **continues running normally**
- Heartbeats will not be sent (no InstanceId available)

## Heartbeat Failure

If a heartbeat request fails:

1. The SDK retries with **exponential backoff**
2. Retry delays: 2s, 4s, 8s, 16s, ...
3. After `RetryAttempts` failures (default: 3), it gives up for that cycle
4. The next heartbeat cycle will try again normally

```
Attempt 1 fails → wait 2s → retry
Attempt 2 fails → wait 4s → retry
Attempt 3 fails → wait 8s → retry
All failed → skip, try again next cycle (30s)
```

## Logging

SDK logs use the standard `ILogger` infrastructure:

| Level | What |
|---|---|
| `Information` | Registration success, heartbeat service start/stop |
| `Warning` | Heartbeat retry, metric collection failure |
| `Error` | Registration failure, heartbeat final failure |
| `Debug` | Individual heartbeat success |

Disable all SDK logging:

```csharp
options.EnableLogging = false;
```

Or filter via `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "ServiceMonitor.Client": "Warning"
    }
  }
}
```
