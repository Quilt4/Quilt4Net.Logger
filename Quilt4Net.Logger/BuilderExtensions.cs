using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quilt4Net.Entities;
using Quilt4Net.Internals;

namespace Quilt4Net;

public static class BuilderExtensions
{
    public static ILoggingBuilder Quilt4NetLogger(this ILoggingBuilder builder, Action<Quilt4NetOptions> options = null)
    {
        builder.Services.AddSingleton<ILoggerProvider>(serviceProvider =>
        {
            var provider = new Quilt4NetProvider(serviceProvider, options);
            //serviceProvider.StartQuilt4NetEngine();
            return provider;
        });
        builder.Services.AddSingleton(serviceProvider =>
        {
            var p = (Quilt4NetProvider)serviceProvider.GetService<ILoggerProvider>();
            return p.ConfigurationData;
        });
        builder.Services.AddSingleton<IConfigurationDataLoader, ConfigurationDataLoader>();
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

    ///// <summary>
    ///// This is normally not needed, the engine will start automatically. But if you are running a console implementation hosted services are not started automatically, this method can be called.
    ///// </summary>
    ///// <param name="serviceProvider"></param>
    //public static void StartQuilt4NetEngine(this IServiceProvider serviceProvider)
    //{
    //    StartQuilt4NetEngine(new ServiceProviderIocProxy(serviceProvider));
    //}

    //public static void StartQuilt4NetEngine(this IIocProxy iocProxy)
    //{
    //    Task.Run(async () =>
    //    {
    //        //TODO: Do not start if already started.

    //        var sw = new Stopwatch();
    //        sw.Start();

    //        //await Task.Delay(TimeSpan.FromSeconds(1)); //Wait for configuration to be loaded

    //        var started = false;
    //        for (var i = 0; i < 5; i++)
    //        {
    //            try
    //            {
    //                var configurationEngine = iocProxy.GetService<IConfigurationEngine>();
    //                await configurationEngine.StartAsync(CancellationToken.None);

    //                var sender = iocProxy.GetService<ISenderEngine>();
    //                await sender.StartAsync(CancellationToken.None);

    //                started = true;
    //                break;
    //            }
    //            catch (ConfigurationException e)
    //            {
    //                await Task.Delay(TimeSpan.FromSeconds(1));
    //            }
    //            catch (Exception e)
    //            {
    //                Debugger.Break();
    //                Console.WriteLine($"{e.Message} @{e.StackTrace}");
    //                break;
    //            }
    //        }

    //        try
    //        {
    //            var configurationDataLoader = iocProxy.GetService<IConfigurationDataLoader>();
    //            var eLogState = started ? ELogState.Debug : ELogState.Warning;
    //            configurationDataLoader.Get().LogEvent?.Invoke(new LogEventArgs(eLogState, null, null, $"The engine was {(started ? "" : "NOT ")}stared.", sw.StopAndGetElapsed()));
    //        }
    //        catch (Exception e)
    //        {
    //            Debugger.Break();
    //            Console.WriteLine($"{e.Message} @{e.StackTrace}");
    //        }
    //    });
    //}
}