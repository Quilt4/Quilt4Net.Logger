using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class ServiceInfoResponse
    {
        public string Version { get; set; }
        public DateTime StartTime { get; set; }
        public string Environment { get; set; }
        public string DatabaseInfo { get; set; }
    }
}