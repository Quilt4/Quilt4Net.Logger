namespace Quilt4Net.Core.Interfaces
{
    public interface IConfiguration
    {
        bool UseBuildTime { get; }
        string ProjectApiKey { get; set; }
        ISessionConfiguration Session { get; }
        ITargetConfiguration Target { get; }
    }
}