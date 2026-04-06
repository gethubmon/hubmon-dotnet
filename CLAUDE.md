# CLAUDE.md — HubMon .NET SDK

## Project Overview
Official .NET client SDK for HubMon service monitoring platform.
- **Package**: `HubMon.Client` on NuGet
- **Namespace**: `ServiceMonitor.Client` (kept for backward compatibility)
- **GitHub Org**: `gethubmon` (https://github.com/gethubmon)
- **Repo**: `gethubmon/hubmon-dotnet`

## Architecture
- `src/HubMon.Client/` — SDK library (net8.0 + net9.0)
- `samples/` — WebApi + WorkerService examples
- `docs/` — VitePress documentation site
- `.github/workflows/` — NuGet publish + docs deploy pipelines

## Key Files
- `ServiceCollectionExtensions.cs` — `AddHubMon()` public API
- `ApplicationBuilderExtensions.cs` — `UseHubMonTracking()` middleware
- `ServiceMonitorOptions.cs` — All configuration options
- `HeartbeatBackgroundService.cs` — Periodic heartbeat + metrics collection
- `RequestTrackingMiddleware.cs` — RPM + active connections tracking
- `ServiceMonitorRegistrar.cs` — Service registration logic

## SDK Features
- Auto service registration on startup
- Periodic heartbeat (default: 30s)
- System metrics: CPU, memory, total memory, disk, thread count
- Request tracking: requests/min, active connections (via middleware)
- Deployment metadata: build ID, commit hash, branch, etc.
- Exponential backoff retry logic

## API Endpoints Used
- `POST /api/services/register` — Service registration (X-API-Key auth)
- `POST /api/services/heartbeat` — Heartbeat with metrics (X-API-Key auth)

## Related Projects
- **API Server**: `e:\service_dashboard` (ikeskin/serv_mon repo)
  - API runs at `https://api.hubmon.com` in production
  - Dev: `http://localhost:5192`
- **VPS**: Hetzner, deploy path `/opt/hubmon`
- **Shared infra**: Redis, Loki, Prometheus, Grafana on `shared-infra` network

## Pipelines
- `nuget-publish.yml` — Tag `v*` → build + pack + publish to GitHub Packages + NuGet.org
- `docs-deploy.yml` — Push to main (docs/ changes) → VitePress build → GitHub Pages

## Versioning
- Tag-based: `v1.0.0-alpha.1` for prerelease, `v1.0.0` for stable
- Current published: `v1.0.0-alpha.1` (NuGet)
- Next milestone: `v1.0.0` stable release

## Commands
```bash
# Build
dotnet build

# Pack
dotnet pack src/HubMon.Client/HubMon.Client.csproj -c Release -o ./nupkg

# Docs dev
cd docs && npm run docs:dev

# Docs build
cd docs && npm run docs:build
```

## NuGet
- API Key secret: `NUGET_API_KEY` (in GitHub repo secrets)
- Prefix reservation: `HubMon.*` on nuget.org
