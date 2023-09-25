namespace Quilt4Net;

public record Configuration
{
    public string Name { get; init; }
    public int LogLevel { get; init; }
}