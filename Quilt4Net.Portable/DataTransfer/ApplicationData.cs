using System;

namespace Quilt4Net.DataTransfer
{
    public class ApplicationData
    {
        public string Fingerprint { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string SupportToolkitNameVersion { get; set; }
        public DateTime? BuildTime { get; set; }
    }
}