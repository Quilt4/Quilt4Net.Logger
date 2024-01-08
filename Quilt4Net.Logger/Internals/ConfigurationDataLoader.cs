using Quilt4Net.Dtos;

namespace Quilt4Net.Internals;

internal class ConfigurationDataLoader : IConfigurationDataLoader
{
    private Func<ConfigurationData> _configurationData;

    public ConfigurationDataLoader()
    {
    }

    public ConfigurationData Get()
    {
        return _configurationData?.Invoke() ?? throw new ConfigurationException("ConfigurationData has not been set.");
    }

    public void Set(Func<ConfigurationData> configurationData)
    {
        if (_configurationData != null) throw new ConfigurationException("ConfigurationData loader has already been set.");
        _configurationData = configurationData;
    }
}