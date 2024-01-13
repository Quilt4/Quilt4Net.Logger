using Microsoft.Extensions.Hosting;

namespace Quilt4Net.Internals;

internal interface IConfigurationEngine : IHostedService
{
    bool Started { get; }
    bool HaveConfiguration { get; }
    event EventHandler<ConfigurationEvent> ConfigurationEvent;
}