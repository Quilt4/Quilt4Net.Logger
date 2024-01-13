namespace Quilt4Net.Internals;

internal class ConfigurationEvent : EventArgs
{
    public ConfigurationEvent(EConfigurationAction configurationAction, Exception exception = null)
    {
        ConfigurationAction = configurationAction;
        Exception = exception;
    }

    public EConfigurationAction ConfigurationAction { get; }
    public Exception Exception { get; }
}