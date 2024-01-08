namespace Quilt4Net.Dtos;

public record StartupRequest
{
    public LogAppData AppData { get; init; }
    public LogSessionData SessionData { get; init; }
}