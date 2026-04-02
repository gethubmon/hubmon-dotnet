# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0] - 2026-04-02

### Added
- Initial release of HubMon .NET SDK
- Automatic service registration on startup
- Periodic heartbeat with configurable interval
- System metrics collection: CPU, memory, disk, thread count
- Request tracking middleware: requests/min, active connections
- Deployment metadata support (CI/CD integration)
- Exponential backoff retry logic
- ASP.NET Core DI integration via `AddServiceMonitor()`
- Auto-detection of hostname, port, and URL
