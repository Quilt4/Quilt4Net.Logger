using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quilt4Net.Internals;
using Quilt4Net.Internals.Standard;

namespace Quilt4Net
{
    public static class BuilderExtensions
    {
        public static ILoggingBuilder Quilt4NetStandardLogger(this ILoggingBuilder builder, Action<Quilt4NetOptions> options = null)
        {
            builder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new Quilt4NetStandardProvider(serviceProvider, options));
            builder.Services.AddSingleton<IConfigurationDataLoader, ConfigurationDataLoader>();
            builder.Services.AddSingleton<ISender>(serviceProvider =>
            {
                var loader = serviceProvider.GetService<IConfigurationDataLoader>();
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                var httpClient = httpClientFactory?.CreateClient("Quilt4Net") ?? new HttpClient();
                return new Sender(loader, httpClient);
            });

            return builder;
        }
    }
}