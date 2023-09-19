using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quilt4Net.Internals;

namespace Quilt4Net;

public static class BuilderExtensions
{
    public static ILoggingBuilder Quilt4NetLogger(this ILoggingBuilder builder, Action<Quilt4NetOptions> options = null)
    {
        builder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new Quilt4NetProvider(serviceProvider, options));
        builder.Services.AddSingleton<IConfigurationDataLoader, ConfigurationDataLoader>();
        builder.Services.AddSingleton<ISender>(serviceProvider =>
        {
            var loader = serviceProvider.GetService<IConfigurationDataLoader>();

            var o = new Quilt4NetOptions { HttpClientLoader = _ => new HttpClient() };
            options?.Invoke(o);
            var httpClient = o.HttpClientLoader?.Invoke(serviceProvider) ?? new HttpClient();

            return new Sender(loader, httpClient);
        });
        return builder;
    }
}