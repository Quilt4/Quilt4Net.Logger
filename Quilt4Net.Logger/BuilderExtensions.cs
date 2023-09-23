using System.Diagnostics;
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
        builder.Services.AddSingleton<ISender, Sender>();
        builder.Services.AddSingleton<ConfigurationEngine>();
        builder.Services.AddHostedService<ConfigurationEngine>();
        return builder;
    }

    /// <summary>
    /// This is normally not needed, the engine will start automatically. But if you are running a console implementation hosted services are not started automatically, this method can be called.
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void StartQuilt4NetConfigurationEngine(this ServiceProvider serviceProvider)
    {
        Task.Run(async () =>
        {
            var sw = new Stopwatch();
            sw.Start();

            await Task.Delay(TimeSpan.FromMilliseconds(100)); //Wait for configuration to be loaded

            bool started = false;
            for(var i = 0; i < 5; i++)
            {
                try
                {
                    var configurationEngine = serviceProvider.GetService<ConfigurationEngine>();
                    await configurationEngine.StartAsync(CancellationToken.None);
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
                configurationDataLoader.Get().LogEvent.Invoke(new LogEventArgs(ELogState.Debug, null, null, $"The configuration engine was {(started ? " " : "NOT ")}stared.", sw.StopAndGetElapsed()));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} @{e.StackTrace}");
            }
        });
    }
}