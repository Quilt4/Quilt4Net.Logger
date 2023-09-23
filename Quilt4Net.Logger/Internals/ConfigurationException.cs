namespace Quilt4Net.Internals;

internal class ConfigurationException : InvalidOperationException
{
    public ConfigurationException(string message)
        : base(message)
    {
    }
}