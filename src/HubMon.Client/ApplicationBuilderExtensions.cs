using Microsoft.AspNetCore.Builder;

namespace ServiceMonitor.Client;

/// <summary>
/// Extension methods for configuring HubMon middleware in the ASP.NET Core pipeline.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds HubMon request tracking middleware to the pipeline.
    /// Tracks requests per minute and active connections.
    /// Must be called when EnableRequestTracking is set to true.
    /// </summary>
    public static IApplicationBuilder UseHubMonTracking(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestTrackingMiddleware>();
    }
}
