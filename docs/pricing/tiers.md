# Pricing Tiers

## Plan Comparison

| Feature | Free | Pro | Enterprise |
|---|---|---|---|
| **Monitoring** | | | |
| Service limit | 3 | 20 | Unlimited |
| Heartbeat interval | 60s | 30s | 10s |
| Data retention | 24 hours | 7 days | 90 days |
| Uptime tracking | Yes | Yes | Yes |
| **Metrics** | | | |
| CPU usage | - | Yes | Yes |
| Memory usage | - | Yes | Yes |
| Disk usage | - | Yes | Yes |
| Thread count | - | Yes | Yes |
| Request/min (RPM) | - | Yes | Yes |
| Active connections | - | Yes | Yes |
| **Alerting** | | | |
| Down/Up notification | Yes | Yes | Yes |
| Degraded notification | - | Yes | Yes |
| **Notification Channels** | | | |
| Email | Yes | Yes | Yes |
| Discord webhook | - | Yes | Yes |
| Slack | - | Yes | Yes |
| Microsoft Teams | - | Yes | Yes |
| Custom webhook | - | Yes | Yes |
| **Dashboard** | | | |
| Service list | Yes | Yes | Yes |
| Live status | Yes | Yes | Yes |
| Metric charts | - | Yes | Yes |
| Incident timeline | - | Yes | Yes |
| **Health Checks** | | | |
| HTTP / TCP | - | Yes | Yes |
| Database | - | Yes | Yes |
| SSL certificate | - | Yes | Yes |
| **Status Page** | | | |
| Public status page | - | Yes | Yes |
| **Team & Access** | | | |
| Team members | 1 | 5 | Unlimited |
| API key limit | 1 | 5 | Unlimited |
| Role-based access (RBAC) | - | Yes | Yes |
| **Deployment** | | | |
| Version tracking | - | Yes | Yes |
| Build metadata | - | Yes | Yes |
| **Support** | | | |
| Support tickets | Yes | Yes | Yes |
| Priority support | - | - | Yes |
| **Price** | $0 | $19/mo | $79/mo |

## Early Adopter Plan

Ilk 50 kullanıcıya özel. Pro planın tüm özellikleri + Enterprise'dan seçili özellikler.

| Feature | Early Adopter |
|---|---|
| Tüm Pro özellikleri | Yes |
| Service limit | Unlimited |
| Heartbeat interval | 10s |
| Data retention | 90 days |
| Team members | Unlimited |
| API key limit | Unlimited |
| Priority support | Yes |
| **Fiyat** | $9/mo (lifetime) |
| **Limit** | İlk 50 kullanıcı |

## Server-Driven Config

Register response returns tier-specific settings:

```json
{
  "instanceId": "...",
  "heartbeatInterval": 30,
  "enabledMetrics": ["cpu", "memory", "disk"],
  "enableRequestTracking": true
}
```

Client ignores user-configured values and uses server-provided config.
Tier upgrades take effect on next registration (no restart needed).

## Tier → Config Mapping

| Config | Free | Pro / Early Adopter | Enterprise |
|---|---|---|---|
| heartbeatInterval | 60 | 30 / 10 | 10 |
| enabledMetrics | [] | ["cpu","memory","disk","threads","rpm","connections"] | same |
| enableRequestTracking | false | true | true |
| maxServices | 3 | 20 / unlimited | unlimited |
| maxTeamMembers | 1 | 5 / unlimited | unlimited |
| maxApiKeys | 1 | 5 / unlimited | unlimited |
| dataRetentionDays | 1 | 7 / 90 | 90 |
