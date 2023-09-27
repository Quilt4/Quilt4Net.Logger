using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quilt4Net.Internals;

namespace Quilt4Net;

public static class BuilderExtensions
{
    public static ILoggingBuilder Quilt4NetBlazorLogger(this ILoggingBuilder builder, Action<Quilt4NetOptions> options = null)
    {
        builder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new Quilt4NetBlazorProvider(serviceProvider, options));
        builder.Services.AddSingleton<IConfigurationDataLoader>(_ => new ConfigurationDataLoader());
        builder.Services.AddSingleton<ISender, Sender>();
        builder.Services.AddHostedService<ConfigurationEngine>();
        builder.Services.AddHttpClient("Quilt4Net.Sender", httpClient =>
        {
            httpClient.BaseAddress = new Uri("");
        });
        return builder;
    }
}