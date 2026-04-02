# Service Registration

## `POST /api/services/register`

Registers a service instance with HubMon. Called automatically by the SDK on application startup.

### Authentication

`X-API-Key` header with your API key.

### Request Body

```json
{
  "serviceName": "payment-api",
  "environment": "production",
  "version": "1.2.3",
  "instanceId": "web-server-1:8080",
  "hostname": "web-server-1",
  "port": 8080,
  "url": "http://web-server-1:8080",
  "processId": 12345,
  "metadata": {
    "sdk_version": "1.0.0",
    "framework": ".NET 8.0.0",
    "os": "Linux 6.1.0",
    "process_id": 12345,
    "registered_at": "2026-04-02T10:00:00Z",
    "commit_hash": "a1b2c3d",
    "branch": "main"
  }
}
```

### Response

```json
{
  "serviceId": "550e8400-e29b-41d4-a716-446655440000",
  "instanceId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
  "serviceName": "payment-api",
  "environment": "production",
  "status": "registered",
  "message": "Service registered successfully"
}
```

### Behavior

- If the service already exists, a new instance is added
- If the instance ID matches an existing instance, it is re-registered (updated)
- `instanceId` is auto-generated from `hostname:port` if not provided
