using Microsoft.Extensions.DependencyInjection;

namespace Quilt4Net.Ioc;

internal class ServiceProviderIocProxy : IIocProxy
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceProviderIocProxy(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T GetService<T>()
    {
        return _serviceProvider.GetService<T>();
    }
}