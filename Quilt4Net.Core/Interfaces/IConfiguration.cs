namespace Quilt4Net.Core.Interfaces
{
    public interface IConfiguration
    {
        string ProjectApiKey { get; set; }
        string ApplicationName { get; set; }
        string ApplicationVersion { get; set; }
        bool UseBuildTime { get; }
        ISessionConfiguration Session { get; }
        ITargetConfiguration Target { get; }
    }
}