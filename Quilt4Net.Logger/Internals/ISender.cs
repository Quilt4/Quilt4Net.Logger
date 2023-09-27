namespace Quilt4Net.Internals;

internal interface ISender
{
    void Send(LogInput logInput);
    Task UpdateConfigurationAsync(CancellationToken cancellationToken);
}