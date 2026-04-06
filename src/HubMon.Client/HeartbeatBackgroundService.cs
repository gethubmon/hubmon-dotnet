using System.Diagnostics;
using System.IO;
using System.Net.Http.Json;
using System.Runtime;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceMonitor.Client.Models;

namespace ServiceMonitor.Client;

internal class HeartbeatBackgroundService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly IServiceMonitorRegistrar _registrar;
    private readonly ServiceMonitorOptions _options;
    private readonly ServerConfig _serverConfig;
    private readonly ILogger<HeartbeatBackgroundService> _logger;
    private readonly Process _currentProcess;

    public HeartbeatBackgroundService(
        HttpClient httpClient,
        IServiceMonitorRegistrar registrar,
        IOptions<ServiceMonitorOptions> options,
        ServerConfig serverConfig,
        ILogger<HeartbeatBackgroundService> logger)
    {
        _httpClient = httpClient;
        _registrar = registrar;
        _options = options.Value;
        _serverConfig = serverConfig;
        _logger = logger;
        _currentProcess = Process.GetCurrentProcess();

        // Configure HttpClient
        _httpClient.BaseAddress = new Uri(_options.DashboardUrl);
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", _options.ApiKey);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Wait for service registration to complete
        await WaitForRegistrationAsync(stoppingToken);

        if (_registrar.InstanceId == null)
        {
            if (_options.EnableLogging)
            {
                _logger.LogError("Service registration failed. Heartbeat service cannot start.");
            }
            return;
        }

        var heartbeatInterval = _serverConfig.GetEffectiveHeartbeatInterval(_options.HeartbeatInterval);

        if (_options.EnableLogging)
        {
            _logger.LogInformation(
                "Heartbeat service started for instance {InstanceId}. Interval: {Interval}s (source: {Source})",
                _registrar.InstanceId,
                heartbeatInterval.TotalSeconds,
                _serverConfig.HeartbeatIntervalSeconds.HasValue ? "server" : "client");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendHeartbeatAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                if (_options.EnableLogging)
                {
                    _logger.LogError(ex, "Error sending heartbeat");
                }
            }

            await Task.Delay(heartbeatInterval, stoppingToken);
        }

        if (_options.EnableLogging)
        {
            _logger.LogInformation("Heartbeat service stopped");
        }
    }

    private async Task WaitForRegistrationAsync(CancellationToken cancellationToken)
    {
        var timeout = TimeSpan.FromSeconds(30);
        var elapsed = TimeSpan.Zero;
        var checkInterval = TimeSpan.FromMilliseconds(100);

        while (_registrar.InstanceId == null && elapsed < timeout)
        {
            await Task.Delay(checkInterval, cancellationToken);
            elapsed += checkInterval;
        }
    }

    private async Task SendHeartbeatAsync(CancellationToken cancellationToken)
    {
        var metadata = new Dictionary<string, object>
        {
            { "timestamp", DateTime.UtcNow }
        };

        // Collect metrics based on server config (falls back to client config)
        var metricsEnabled = _serverConfig.HasAnyMetrics() || _options.EnableMetrics;

        if (metricsEnabled)
        {
            try
            {
                _currentProcess.Refresh();

                bool ServerMetric(string key) => _serverConfig.HasAnyMetrics()
                    ? _serverConfig.IsMetricEnabled(key)
                    : _options.EnableMetrics;

                if (ServerMetric("cpu"))
                {
                    var cpuTime = _currentProcess.TotalProcessorTime;
                    var uptime = DateTime.UtcNow - _currentProcess.StartTime.ToUniversalTime();
                    var cpuPercent = (cpuTime.TotalMilliseconds / uptime.TotalMilliseconds) * 100.0 / Environment.ProcessorCount;
                    metadata["cpu_percent"] = Math.Round(cpuPercent, 2);
                }

                if (ServerMetric("memory"))
                {
                    var memoryMB = _currentProcess.WorkingSet64 / 1024.0 / 1024.0;
                    metadata["memory_mb"] = Math.Round(memoryMB, 2);

                    var gcMemInfo = GC.GetGCMemoryInfo();
                    var totalMemoryMB = gcMemInfo.TotalAvailableMemoryBytes / 1024.0 / 1024.0;
                    metadata["total_memory_mb"] = Math.Round(totalMemoryMB, 2);
                }

                if (ServerMetric("disk"))
                {
                    try
                    {
                        var rootDrive = new DriveInfo(Path.GetPathRoot(AppContext.BaseDirectory) ?? "/");
                        if (rootDrive.IsReady)
                        {
                            metadata["disk_usage_percent"] = Math.Round((1.0 - (double)rootDrive.AvailableFreeSpace / rootDrive.TotalSize) * 100, 2);
                        }
                    }
                    catch { /* Disk info not available */ }
                }

                if (ServerMetric("threads"))
                {
                    metadata["thread_count"] = _currentProcess.Threads.Count;
                }
            }
            catch (Exception ex)
            {
                if (_options.EnableLogging)
                {
                    _logger.LogWarning(ex, "Failed to collect process metrics");
                }
            }
        }

        // Request tracking metrics — server config overrides client config
        var requestTrackingEnabled = _serverConfig.GetEffectiveRequestTracking(_options.EnableRequestTracking);

        if (requestTrackingEnabled)
        {
            if (!_serverConfig.HasAnyMetrics() || _serverConfig.IsMetricEnabled("rpm"))
                metadata["requests_per_minute"] = RequestTracker.GetRequestsPerMinute();

            if (!_serverConfig.HasAnyMetrics() || _serverConfig.IsMetricEnabled("connections"))
                metadata["active_connections"] = RequestTracker.GetActiveConnections();
        }

        var request = new HeartbeatRequest
        {
            InstanceId = _registrar.InstanceId!.Value,
            Metadata = metadata
        };

        var retryCount = 0;
        var maxRetries = _options.RetryAttempts;

        while (retryCount <= maxRetries)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/services/heartbeat", request, cancellationToken);
                response.EnsureSuccessStatusCode();

                if (_options.EnableLogging)
                {
                    _logger.LogDebug("Heartbeat sent successfully for instance {InstanceId}", _registrar.InstanceId);
                }

                return;
            }
            catch (HttpRequestException ex)
            {
                retryCount++;

                if (retryCount > maxRetries)
                {
                    if (_options.EnableLogging)
                    {
                        _logger.LogError(ex, "Failed to send heartbeat after {RetryCount} attempts", maxRetries);
                    }
                    return;
                }

                // Exponential backoff
                var delay = TimeSpan.FromSeconds(Math.Pow(2, retryCount));
                if (_options.EnableLogging)
                {
                    _logger.LogWarning(
                        "Heartbeat failed (attempt {Attempt}/{MaxAttempts}). Retrying in {Delay}s...",
                        retryCount,
                        maxRetries,
                        delay.TotalSeconds);
                }

                await Task.Delay(delay, cancellationToken);
            }
        }
    }
}
