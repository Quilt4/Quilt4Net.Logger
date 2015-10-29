using System;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class SessionResponse
    {
        public Guid SessionKey { get; set; }
        public string ApplicationName { get; set; }
        public string VersionName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string EnvironmentName { get; set; }
        public string EnvironmentColor { get; set; }
        public string CallerIpAddress { get; set; }
        public string UserKey { get; set; }
        public string UserHandleName { get; set; }
        public string MachineKey { get; set; }
    }
}