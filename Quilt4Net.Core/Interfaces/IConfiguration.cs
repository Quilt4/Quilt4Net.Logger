namespace Quilt4Net.Core.Interfaces
{
    public interface IConfiguration
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