namespace Quilt4Net;

//NOTE: One record per compiled version of the assembly
public record LogAppData
{
    public string Application { get; init; }
    public string Version { get; init; }
    public string LoggerInfo { get; init; }
}

public record StartupRequest
{
    public LogAppData AppData { get; init; }
    public LogSessionData SessionData { get; init; }
}

public record StartupResponse
{
    public string AppDataKey { get; init; }
    public string SessionDataKey { get; init; }
    public Configuration Configuration { get; init; }
}

public record EndRequest
{

}