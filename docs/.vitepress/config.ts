import { defineConfig } from 'vitepress'

export default defineConfig({
  title: 'HubMon SDK',
  description: 'Official documentation for the HubMon .NET monitoring SDK',
  base: '/hubmon-dotnet/',
  themeConfig: {
    nav: [
      { text: 'Guide', link: '/guide/getting-started' },
      { text: 'Examples', link: '/examples/aspnet-web-api' },
      { text: 'API Reference', link: '/api/service-registration' },
      { text: 'GitHub', link: 'https://github.com/gethubmon/hubmon-dotnet' }
    ],
    sidebar: [
      {
        text: 'Guide',
        items: [
          { text: 'Getting Started', link: '/guide/getting-started' },
          { text: 'Configuration', link: '/guide/configuration' },
          { text: 'How It Works', link: '/guide/how-it-works' },
          { text: 'Metrics', link: '/guide/metrics' },
          { text: 'Deployment Metadata', link: '/guide/deployment-metadata' },
          { text: 'Error Handling', link: '/guide/error-handling' }
        ]
      },
      {
        text: 'Examples',
        items: [
          { text: 'ASP.NET Core Web API', link: '/examples/aspnet-web-api' },
          { text: 'Worker Service', link: '/examples/worker-service' },
          { text: 'Using appsettings.json', link: '/examples/appsettings' },
          { text: 'Docker', link: '/examples/docker' }
        ]
      },
      {
        text: 'API Reference',
        items: [
          { text: 'Service Registration', link: '/api/service-registration' },
          { text: 'Heartbeat', link: '/api/heartbeat' },
          { text: 'Authentication', link: '/api/authentication' }
        ]
      },
      {
        text: 'SDK Reference',
        items: [
          { text: 'Options', link: '/reference/options' },
          { text: 'Models', link: '/reference/models' }
        ]
      }
    ],
    socialLinks: [
      { icon: 'github', link: 'https://github.com/gethubmon/hubmon-dotnet' }
    ],
    search: {
      provider: 'local'
    },
    footer: {
      message: 'Released under the MIT License.',
      copyright: 'Copyright 2026 HubMon'
    }
  }
})
