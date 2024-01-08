using Microsoft.Extensions.Hosting;
using Quilt4Net.Dtos;

namespace Quilt4Net.Internals;

internal interface ISenderEngine : IHostedService, IDisposable
{
    Task<Configuration> GetConfigurationAsync(CancellationToken cancellationToken);
}