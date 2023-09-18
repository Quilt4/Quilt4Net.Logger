namespace Quilt4Net.Internals;

internal class ConfigurationDataLoader : IConfigurationDataLoader
{
    private Func<ConfigurationData> _configurationData;

    public ConfigurationData Get()
    {
        return _configurationData?.Invoke() ?? throw new InvalidOperationException("ConfigurationData has not been set.");
    }

    public void Set(Func<ConfigurationData> configurationData)
    {
        if (_configurationData != null) throw new InvalidOperationException("ConfigurationData has already been set.");
        _configurationData = configurationData;
    }
}