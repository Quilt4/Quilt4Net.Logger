namespace Tharga.Quilt4Net.Interfaces
{
    public interface IArchiveInfo
    {
        int VersionCount { get; }
        int SessionCount { get; }
        int IssueTypeCount { get; }
        int IssueCount { get; }
    }
}