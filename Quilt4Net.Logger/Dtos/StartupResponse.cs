namespace Quilt4Net.Dtos;

public record StartupResponse
{
    public string AppDataKey { get; init; }
    public string SessionDataKey { get; init; }
    public Configuration Configuration { get; init; }
}