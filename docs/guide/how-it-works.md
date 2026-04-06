# How It Works

## Architecture

The SDK runs two background services inside your application:

```
Your App starts
    │
    ├── ServiceRegistrationHostedService
    │   └── POST /api/services/register
    │       → Returns InstanceId
    │
    └── HeartbeatBackgroundService (loop)
        └── POST /api/services/heartbeat
            → Sends InstanceId + metrics every 30s
```

## Registration Flow

1. Your application starts and `AddHubMon()` configures the SDK
2. `ServiceRegistrationHostedService` runs on startup
3. The SDK auto-detects hostname, port, and URL
4. Sends a `POST /api/services/register` request with service metadata
5. HubMon returns a unique `InstanceId`

## Heartbeat Flow

1. `HeartbeatBackgroundService` waits for registration to complete (up to 30s)
2. Once registered, it enters a loop at the configured interval (default: 30s)
3. Each heartbeat includes:
   - Instance ID
   - Timestamp
   - System metrics (if enabled)
   - Request tracking data (if enabled)
4. If a heartbeat fails, it retries with exponential backoff

## Status Detection

HubMon determines service status based on heartbeats:

| Condition | Status |
|---|---|
| Heartbeat received within expected interval | **Healthy** |
| Heartbeat delayed (1-2 missed intervals) | **Degraded** |
| No heartbeat for 2+ minutes | **Down** |
| Heartbeat resumes after being down | **Recovered** |

When a service goes down, HubMon automatically creates an incident and sends notifications to configured channels (Slack, Discord, email, etc.).
