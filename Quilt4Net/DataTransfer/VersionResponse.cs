using System;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class VersionResponse
    {
        public string Name { get; set; }
        public string ApplicationName { get; set; }
        public DateTime? BuildTime { get; set; }
        public string SupportToolkit { get; set; }
    }
}