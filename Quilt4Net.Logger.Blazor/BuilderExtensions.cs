using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quilt4Net.Entities;
using Quilt4Net.Internals;

namespace Quilt4Net;

public static class BuilderExtensions
{
    public static ILoggingBuilder Quilt4NetBlazorLogger(this ILoggingBuilder builder, Action<Quilt4NetOptions> options = null)
    {
        builder.Services.AddSingleton<ILoggerProvider>(serviceProvider =>
        {
            serviceProvider.StartQuilt4NetEngine();
            return new Quilt4NetBlazorProvider(serviceProvider, options);
        });
        builder.Services.AddSingleton<IConfigurationDataLoader, ConfigurationDataLoader>();
        builder.Services.AddSingleton<IMessageQueue, MessageQueue>();
        builder.Services.AddSingleton<ISenderEngine, SenderEngine>();
        builder.Services.AddSingleton<ConfigurationEngine>();

        builder.Services.AddHostedService<SenderEngine>();
        builder.Services.AddHostedService<ConfigurationEngine>();

        return builder;
    }
}