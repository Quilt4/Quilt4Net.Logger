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
        builder.Services.AddHttpClient();
        //builder.Services.AddSingleton<ISender>(serviceProvider =>
        //{
        //    var loader = serviceProvider.GetService<IConfigurationDataLoader>();
        //    HttpClient httpClient;
        //    try
        //    {
        //        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        //        httpClient = httpClientFactory?.CreateClient("Quilt4Net") ?? new HttpClient();
        //    }
        //    catch (Exception e)
        //    {
        //        httpClient = new HttpClient();
        //    }
        //    return new Sender(loader, httpClient);
        //});
        return builder;
    }
}