# Docker

## Docker Compose

```yaml
services:
  my-api:
    image: my-api:latest
    environment:
      - HubMon__DashboardUrl=https://api.hubmon.com
      - HubMon__ApiKey=sm_live_your_key
      - HubMon__ServiceName=my-api
      - HubMon__Environment=production
      - HubMon__EnableMetrics=true
      - HubMon__EnableRequestTracking=true
```

## Kubernetes

```yaml
apiVersion: apps/v1
kind: Deployment
spec:
  template:
    spec:
      containers:
        - name: my-api
          env:
            - name: HubMon__DashboardUrl
              value: "https://api.hubmon.com"
            - name: HubMon__ApiKey
              valueFrom:
                secretKeyRef:
                  name: hubmon-secrets
                  key: api-key
            - name: HubMon__ServiceName
              value: "my-api"
            - name: HubMon__Environment
              value: "production"
            - name: HubMon__EnableMetrics
              value: "true"
```

## Instance Identification

In containerized environments, the SDK auto-detects the container hostname. Each container gets a unique identity based on:

1. `Hostname` (container ID by default)
2. `Port` (auto-detected from Kestrel)
3. `ProcessId`

You can also set a custom `InstanceId`:

```csharp
options.InstanceId = Environment.GetEnvironmentVariable("HOSTNAME");
```
