using Quilt4Net.Dtos;

namespace Quilt4Net.Internals;

[Obsolete("Use ConfigurationData or IConfigurationData instead.")]
internal class ConfigurationDataLoader : IConfigurationDataLoader
{
    private Func<ConfigurationData> _configurationDataLoader;
    private ConfigurationData _configurationData;

    public ConfigurationData Get()
    {
        return _configurationData ??= _configurationDataLoader?.Invoke() ?? throw new ConfigurationException("ConfigurationDataLoader has not been set yet.");
    }

    public void Set(Func<ConfigurationData> configurationData)
    {
        if (_configurationDataLoader != null) throw new ConfigurationException("ConfigurationData loader has already been set.");
        _configurationDataLoader = configurationData;
    }
}