namespace Quilt4Net.Dtos;

public record ErrorMessage
{
    public Guid CorrelationId { get; init; }
    public string Message { get; init; }
}