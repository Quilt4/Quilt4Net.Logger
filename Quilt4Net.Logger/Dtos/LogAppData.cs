namespace Quilt4Net.Dtos;

//NOTE: One record per compiled version of the assembly
public record LogAppData
{
    public string Application { get; init; }
    public string Version { get; init; }
    public string LoggerInfo { get; init; }
}