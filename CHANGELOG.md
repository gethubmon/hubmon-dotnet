# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2026-04-06

### Changed
- Renamed `AddServiceMonitor()` to `AddHubMon()` for consistency with package branding

### Fixed
- Fixed VitePress docs build (ESM module compatibility)

## [1.0.0] - 2026-04-04

Stable release — no changes from `1.0.0-alpha.1`.

## [1.0.0-alpha.1] - 2026-04-04

### Added
- Initial public pre-release of HubMon .NET SDK
- Automatic service registration on startup
- Periodic heartbeat with configurable interval (default: 30s)
- System metrics collection: CPU, memory, disk, thread count
- Request tracking middleware: requests/min, active connections
- Deployment metadata support (CI/CD integration)
- Exponential backoff retry logic (configurable 0–10 attempts)
- ASP.NET Core DI integration via `AddHubMon()`
- Request tracking middleware via `UseHubMonTracking()`
- Auto-detection of hostname, port, and URL
- Multi-target support: .NET 8.0 and .NET 9.0
