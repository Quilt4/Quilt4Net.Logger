using System;

namespace Quilt4Net
{
    public interface IConfigurationDataLoader
    {
        ConfigurationData Get();
        void Set(Func<ConfigurationData> configurationData);
    }
}