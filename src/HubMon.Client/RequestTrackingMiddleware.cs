using Microsoft.AspNetCore.Http;

namespace ServiceMonitor.Client;

/// <summary>
/// Middleware that tracks HTTP request counts and active connections for monitoring.
/// </summary>
internal class RequestTrackingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestTrackingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Interlocked.Increment(ref RequestTracker.ActiveConnections);
        Interlocked.Increment(ref RequestTracker.TotalRequests);

        try
        {
            await _next(context);
        }
        finally
        {
            Interlocked.Decrement(ref RequestTracker.ActiveConnections);
        }
    }
}

/// <summary>
/// Static counters for request tracking. Thread-safe via Interlocked operations.
/// </summary>
internal static class RequestTracker
{
    internal static long ActiveConnections;
    internal static long TotalRequests;
    private static long _lastTotalRequests;
    private static DateTime _lastCalculation = DateTime.UtcNow;

    /// <summary>
    /// Calculates requests per minute based on the delta since last call.
    /// </summary>
    internal static long GetRequestsPerMinute()
    {
        var now = DateTime.UtcNow;
        var currentTotal = Interlocked.Read(ref TotalRequests);
        var lastTotal = Interlocked.Exchange(ref _lastTotalRequests, currentTotal);
        var elapsed = (now - _lastCalculation).TotalMinutes;
        _lastCalculation = now;

        if (elapsed <= 0) return 0;
        return (long)((currentTotal - lastTotal) / elapsed);
    }

    /// <summary>
    /// Gets the current number of active connections.
    /// </summary>
    internal static long GetActiveConnections() => Interlocked.Read(ref ActiveConnections);
}
