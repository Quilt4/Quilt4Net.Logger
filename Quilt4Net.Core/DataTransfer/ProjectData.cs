using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class ProjectData
    {
        internal ProjectData()
        {
        }

        public Guid ProjectKey { get; set; }
        public string Name { get; set; }
        public string DashboardColor { get; set; }
        public string ProjectApiKey { get; set; }
    }
}