using Quilt4Net.Dtos;

namespace Quilt4Net.Internals;

internal interface IConfigurationDataLoader
{
    ConfigurationData Get();
    void Set(Func<ConfigurationData> configurationData);
}