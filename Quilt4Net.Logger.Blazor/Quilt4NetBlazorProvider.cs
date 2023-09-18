﻿using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Quilt4Net;

[ProviderAlias("Quilt4NetBlazorLogger")]
internal class Quilt4NetBlazorProvider : Quilt4NetProvider
{
    public Quilt4NetBlazorProvider(IServiceProvider serviceProvider, Action<Quilt4NetOptions> options)
        : base(serviceProvider, options)
    {
    }

    protected override (string EnvironmentName, string ApplicationName) GetAppName()
    {
        var hostEnvironment = _serviceProvider.GetService<IWebAssemblyHostEnvironment>();
        return (hostEnvironment.Environment, null);
    }
}