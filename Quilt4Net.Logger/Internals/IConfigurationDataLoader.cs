using Quilt4Net.Dtos;

namespace Quilt4Net.Internals;

[Obsolete("Use IConfigurationData instead.")]
internal interface IConfigurationDataLoader
{
    ConfigurationData Get();
    void Set(Func<ConfigurationData> configurationData);
}