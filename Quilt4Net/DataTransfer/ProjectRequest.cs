using System;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class CreateProjectRequest
    {
        public Guid Key { get; set; }
        public string Name { get; set; }
        public string DashboardColor { get; set; }
    }
}