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
            var provider = new Quilt4NetBlazorProvider(serviceProvider, options);
            //serviceProvider.StartQuilt4NetEngine();
            return provider;
        });
        builder.Services.AddSingleton(serviceProvider =>
        {
            var p = (Quilt4NetProvider)serviceProvider.GetService<ILoggerProvider>();
            return p.ConfigurationData;
        });
        //builder.Services.AddSingleton<IConfigurationDataLoader, ConfigurationDataLoader>();
        builder.Services.AddSingleton<IMessageQueue, MessageQueue>();
        builder.Services.AddSingleton<IStateService>(serviceProvider =>
        {
            var configurationEngine = serviceProvider.GetService<IConfigurationEngine>();
            var senderEngine = serviceProvider.GetService<ISenderEngine>();
            var messageQueue = serviceProvider.GetService<IMessageQueue>();
            var p = (Quilt4NetProvider)serviceProvider.GetService<ILoggerProvider>();
            return new StateService(configurationEngine, senderEngine, messageQueue, p.Options);
        });
        builder.Services.AddSingleton<ISenderEngine, SenderEngine>();
        builder.Services.AddSingleton<IConfigurationEngine, ConfigurationEngine>();

        builder.Services.AddHostedService<SenderEngine>();
        builder.Services.AddHostedService<ConfigurationEngine>();

        return builder;
    }
}