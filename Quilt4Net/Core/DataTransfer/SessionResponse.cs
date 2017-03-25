using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class SessionResponse
    {
        public string SessionKey { get; set; }
        public DateTime ServerStartTime { get; set; }
        public string SessionUrl { get; set; }
    }
}