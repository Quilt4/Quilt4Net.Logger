using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class ApplicationData
    {
        internal ApplicationData()
        {
        }

        public string Fingerprint { get; internal set; }
        public string Name { get; internal set; }
        public string Version { get; internal set; }
        public string SupportToolkitNameVersion { get; internal set; }
        public DateTime? BuildTime { get; internal set; }
    }
}