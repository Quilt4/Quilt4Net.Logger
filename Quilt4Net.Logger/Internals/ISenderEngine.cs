using Microsoft.Extensions.Hosting;

namespace Quilt4Net.Internals;

internal interface ISenderEngine : IHostedService, IDisposable
{
    Task UpdateConfigurationAsync(CancellationToken cancellationToken);
}