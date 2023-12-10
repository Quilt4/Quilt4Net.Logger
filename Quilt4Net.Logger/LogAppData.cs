namespace Quilt4Net;

//NOTE: One record per compiled version of the assembly
public record LogAppData
{
    public string Application { get; init; }
    public string Version { get; init; }
    //public LogDataItem[] Data { get; init; }
    public string LoggerInfo { get; init; }
}