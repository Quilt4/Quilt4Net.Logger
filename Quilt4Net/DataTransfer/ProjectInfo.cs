using System;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class ProjectInfo : IProjectInfo
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public int VersionCount { get; set; }
        public int SessionCount { get; set; }
        public int IssueTypeCount { get; set; }
        public int IssueCount { get; set; }
        public string DashboardColor { get; set; }
    }
}