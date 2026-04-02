# Deployment Metadata

Track CI/CD information with each service registration. This helps you identify which build, commit, and branch each instance is running.

## Basic Usage

```csharp
builder.Services.AddServiceMonitor(options =>
{
    options.DashboardUrl = "https://api.hubmon.com";
    options.ApiKey = "sm_live_...";
    options.ServiceName = "payment-api";
    options.Version = "1.2.3";

    // Deployment metadata
    options.CommitHash = "a1b2c3d4e5f6789";
    options.Branch = "main";
    options.BuildConfiguration = "Release";
    options.DeploymentDate = DateTime.UtcNow;
});
```

## Azure DevOps Integration

```csharp
builder.Services.AddServiceMonitor(options =>
{
    options.DashboardUrl = "https://api.hubmon.com";
    options.ApiKey = "sm_live_...";
    options.ServiceName = "payment-api";
    options.Version = Environment.GetEnvironmentVariable("BUILD_BUILDNUMBER");

    options.BuildId = Environment.GetEnvironmentVariable("BUILD_BUILDID");
    options.ReleaseId = Environment.GetEnvironmentVariable("RELEASE_RELEASEID");
    options.CommitHash = Environment.GetEnvironmentVariable("BUILD_SOURCEVERSION");
    options.Branch = Environment.GetEnvironmentVariable("BUILD_SOURCEBRANCH");
    options.BuildConfiguration = Environment.GetEnvironmentVariable("BUILDCONFIGURATION");
    options.DeploymentDate = DateTime.UtcNow;
});
```

## GitHub Actions Integration

```csharp
options.CommitHash = Environment.GetEnvironmentVariable("GITHUB_SHA");
options.Branch = Environment.GetEnvironmentVariable("GITHUB_REF_NAME");
options.BuildId = Environment.GetEnvironmentVariable("GITHUB_RUN_ID");
options.DeploymentDate = DateTime.UtcNow;
```

## Custom Metadata

Add any key-value pairs with `DeploymentMetadata`:

```csharp
options.DeploymentMetadata = new Dictionary<string, object>
{
    { "deployer", "ci-pipeline" },
    { "region", "eu-west-1" },
    { "cluster", "production-k8s" }
};
```

Custom metadata keys are prefixed with `custom_` in the API.
