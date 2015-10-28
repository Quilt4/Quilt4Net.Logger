using System;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IProjectInfo
    {
        Guid ProjectId { get; }
        string Name { get; }
        int VersionCount { get; }
        int SessionCount { get; }
        int IssueTypeCount { get; }
        int IssueCount { get; }
        string DashboardColor { get; }
    }
}