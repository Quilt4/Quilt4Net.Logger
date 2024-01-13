using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quilt4Net.Entities;
using Quilt4Net.Internals;
using Quilt4Net.Ioc;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.Sinks.Quilt4Net;

public static class LoggerConfigurationExtensions
{
    public static LoggerConfiguration Quilt4Net(this LoggerSinkConfiguration sinkConfiguration, Action<Quilt4NetOptions> options = null, IConfiguration configuration = null, IHostEnvironment hostEnvironment = null)
    {
        if (sinkConfiguration == null) throw new ArgumentNullException(nameof(sinkConfiguration));

        var instanceContainer = new InstanceContainer(configuration, hostEnvironment, options);
        instanceContainer.GetService<ILoggerProvider>();
        //instanceContainer.StartQuilt4NetEngine();

        return sinkConfiguration.Sink(new Quilt4NetSink(instanceContainer.GetService<IMessageQueue>()), LogEventLevel.Verbose);
    }
}