using System;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class ProjectData
    {
        public Guid ProjectKey { get; set; }
        public string Name { get; set; }
        public string DashboardColor { get; set; }
        public string ProjectApiKey { get; set; }
    }
}