namespace Quilt4Net.Core.Interfaces
{
    public interface IHashHandler
    {
        string ToMd5Hash(string input);
    }

    public interface IConfigurationHandler
    {
        bool Enabled { get; set; }
        string ProjectApiKey { get; set; }
        string ApplicationName { get; set; }
        string ApplicationVersion { get; set; }
        bool UseBuildTime { get; }
        ISessionConfiguration Session { get; }
        ITargetConfiguration Target { get; }
    }
}