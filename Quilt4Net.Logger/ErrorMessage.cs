namespace Quilt4Net;

public record ErrorMessage
{
    public Guid CorrelationId { get; init; }
    public string Message { get; init; }
}