# Heartbeat

## `POST /api/services/heartbeat`

Sends a heartbeat to indicate the service instance is alive. Called periodically by the SDK (default: every 30 seconds).

### Authentication

`X-API-Key` header with your API key.

### Request Body

```json
{
  "instanceId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
  "metadata": {
    "timestamp": "2026-04-02T10:01:00Z",
    "cpu_percent": 12.5,
    "memory_mb": 256.3,
    "total_memory_mb": 16384.0,
    "disk_usage_percent": 45.2,
    "thread_count": 24,
    "requests_per_minute": 1250,
    "active_connections": 42
  }
}
```

### Response

```json
{
  "status": "ok",
  "timestamp": "2026-04-02T10:01:00Z",
  "message": "Heartbeat received"
}
```

### Metadata Fields

| Field | Type | Condition |
|---|---|---|
| `timestamp` | `DateTime` | Always included |
| `cpu_percent` | `double` | When `EnableMetrics = true` |
| `memory_mb` | `double` | When `EnableMetrics = true` |
| `total_memory_mb` | `double` | When `EnableMetrics = true` |
| `disk_usage_percent` | `double` | When `EnableMetrics = true` |
| `thread_count` | `int` | When `EnableMetrics = true` |
| `requests_per_minute` | `long` | When `EnableRequestTracking = true` |
| `active_connections` | `long` | When `EnableRequestTracking = true` |
