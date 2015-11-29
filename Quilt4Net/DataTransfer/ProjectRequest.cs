using System;

namespace Tharga.Quilt4Net.DataTransfer
{
    internal class UpdateProjectRequest
    {
        public Guid ProjectKey { get; set; }
        public string Name { get; set; }
        public string DashboardColor { get; set; }
    }
}