using System;

namespace Tharga.Quilt4Net.DataTransfer
{
    internal class CreateProjectRequest
    {
        public Guid ProjectKey { get; set; }
        public string Name { get; set; }
        public string DashboardColor { get; set; }
    }
}