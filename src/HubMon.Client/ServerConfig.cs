namespace ServiceMonitor.Client;

/// <summary>
/// Holds server-driven configuration received from the register response.
/// Shared as a singleton between registrar and heartbeat service.
/// </summary>
internal class ServerConfig
{
    /// <summary>
    /// Heartbeat interval in seconds, as dictated by the server (tier-based).
    /// Null means server did not provide a value — use client fallback.
    /// </summary>
    public int? HeartbeatIntervalSeconds { get; set; }

    /// <summary>
    /// List of metric keys the server allows this tier to collect.
    /// Null or empty means no metrics should be collected.
    /// Valid keys: "cpu", "memory", "disk", "threads", "rpm", "connections"
    /// </summary>
    public string[]? EnabledMetrics { get; set; }

    /// <summary>
    /// Whether the server allows request tracking for this tier.
    /// </summary>
    public bool? EnableRequestTracking { get; set; }

    /// <summary>
    /// Returns the effective heartbeat interval, using server value if available,
    /// otherwise falling back to the client-configured value.
    /// </summary>
    public TimeSpan GetEffectiveHeartbeatInterval(TimeSpan clientFallback)
    {
        return HeartbeatIntervalSeconds.HasValue
            ? TimeSpan.FromSeconds(HeartbeatIntervalSeconds.Value)
            : clientFallback;
    }

    /// <summary>
    /// Returns whether a specific metric should be collected.
    /// If server provided enabledMetrics, only those in the list are allowed.
    /// If server didn't provide it, falls back to client config.
    /// </summary>
    public bool IsMetricEnabled(string metricKey)
    {
        if (EnabledMetrics == null) return false;
        return Array.Exists(EnabledMetrics, m => string.Equals(m, metricKey, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Returns whether any system metrics should be collected.
    /// </summary>
    public bool HasAnyMetrics()
    {
        return EnabledMetrics != null && EnabledMetrics.Length > 0;
    }

    /// <summary>
    /// Returns whether request tracking is enabled based on server config.
    /// Falls back to client config if server didn't specify.
    /// </summary>
    public bool GetEffectiveRequestTracking(bool clientFallback)
    {
        return EnableRequestTracking ?? clientFallback;
    }
}
