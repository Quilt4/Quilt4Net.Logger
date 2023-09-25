namespace Quilt4Net.Internals;

internal interface ISender : IDisposable
{
    void Send(LogInput logInput);
    Task UpdateConfigurationAsync(CancellationToken cancellationToken);
}