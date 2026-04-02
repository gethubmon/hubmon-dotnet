# Authentication

## API Keys

The SDK authenticates with HubMon using API keys. Keys are sent via the `X-API-Key` HTTP header.

### Key Format

API keys follow the format: `sm_live_` followed by 32 random characters.

```
sm_live_TMXFO0CkgUQg2kLiCdZEHi2dGIgLBGTV
```

### Creating an API Key

1. Log in to your HubMon dashboard
2. Navigate to **Settings > API Keys**
3. Click **Create New Key**
4. Give it a descriptive name (e.g., "Production API")
5. Copy the key immediately

::: warning
API keys are shown only once at creation. Store them securely (environment variables, secrets manager).
:::

### Security Best Practices

- **Never commit API keys** to source control
- Use **environment variables** or a **secrets manager**
- Create **separate keys** for each environment (dev, staging, prod)
- **Rotate keys** periodically
- Use the **minimum required permissions**

### Example: Using Environment Variables

```csharp
options.ApiKey = Environment.GetEnvironmentVariable("HUBMON_API_KEY")
    ?? throw new Exception("HUBMON_API_KEY is not set");
```

### Example: Using .NET User Secrets (Development)

```bash
dotnet user-secrets set "HubMon:ApiKey" "sm_live_your_key"
```
