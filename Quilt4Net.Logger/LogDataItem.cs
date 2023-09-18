namespace Quilt4Net;

public record LogDataItem
{
    public string Key { get; init; }
    public string Type { get; init; }
    public string Value { get; init; }
}