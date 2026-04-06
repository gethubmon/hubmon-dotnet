using ServiceMonitor.Client;

var builder = WebApplication.CreateBuilder(args);

// Add HubMon monitoring
builder.Services.AddHubMon(options =>
{
    options.DashboardUrl = builder.Configuration["HubMon:DashboardUrl"] ?? "http://localhost:5192";
    options.ApiKey = builder.Configuration["HubMon:ApiKey"] ?? "sm_live_your_key_here";
    options.ServiceName = "sample-webapi";
    options.Environment = builder.Environment.EnvironmentName.ToLower();
    options.Version = "1.0.0";
    options.HeartbeatInterval = TimeSpan.FromSeconds(15);
    options.EnableMetrics = true;
    options.EnableRequestTracking = true;
    options.EnableLogging = true;
});

var app = builder.Build();

// Enable request tracking middleware
app.UseHubMonTracking();

app.MapGet("/", () => "HubMon Sample Web API is running!");

app.MapGet("/weatherforecast", () =>
{
    var summaries = new[] { "Freezing", "Cool", "Mild", "Warm", "Hot" };
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ));
    return forecast;
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
