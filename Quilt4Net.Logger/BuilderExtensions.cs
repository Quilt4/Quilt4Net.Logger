using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quilt4Net.Dtos;
using Quilt4Net.Entities;
using Quilt4Net.Internals;

namespace Quilt4Net;

public static class BuilderExtensions
{
    public static ILoggingBuilder Quilt4NetLogger(this ILoggingBuilder builder, Action<Quilt4NetOptions> options = null)
    {
        builder.Services.AddSingleton<ILoggerProvider>(serviceProvider =>
        {
            serviceProvider.StartQuilt4NetEngine();
            return new Quilt4NetProvider(serviceProvider, options);
        });
        builder.Services.AddSingleton<IConfigurationDataLoader, ConfigurationDataLoader>();
        builder.Services.AddSingleton<IMessageQueue, MessageQueue>();
        builder.Services.AddSingleton<ISenderEngine, SenderEngine>();
        builder.Services.AddSingleton<ConfigurationEngine>();

        builder.Services.AddHostedService<SenderEngine>();
        builder.Services.AddHostedService<ConfigurationEngine>();

        return builder;
    }

    /// <summary>
    /// This is normally not needed, the engine will start automatically. But if you are running a console implementation hosted services are not started automatically, this method can be called.
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void StartQuilt4NetEngine(this IServiceProvider serviceProvider)
    {
        Task.Run(async () =>
        {
            var sw = new Stopwatch();
            sw.Start();

            await Task.Delay(TimeSpan.FromMilliseconds(200)); //Wait for configuration to be loaded

            var started = false;
            for(var i = 0; i < 5; i++)
            {
                try
                {
                    var configurationEngine = serviceProvider.GetService<ConfigurationEngine>();
                    await configurationEngine.StartAsync(CancellationToken.None);

                    var sender = serviceProvider.GetService<ISenderEngine>();
                    await sender.StartAsync(CancellationToken.None);

                    started = true;
                    break;
                }
                catch (ConfigurationException e)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }

            try
            {
                var configurationDataLoader = serviceProvider.GetService<IConfigurationDataLoader>();
                configurationDataLoader.Get().LogEvent?.Invoke(new LogEventArgs(ELogState.Debug, null, null, $"The engine was {(started ? "" : "NOT ")}stared.", sw.StopAndGetElapsed()));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} @{e.StackTrace}");
            }
        });
    }
}