using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class ProjectRequest
    {
        internal ProjectRequest()
        {
        }

        public Guid ProjectKey { get; internal set; }
        public string Name { get; internal set; }
        public string DashboardColor { get; internal set; }
        public string ProjectApiKey { get; internal set; }
    }
}