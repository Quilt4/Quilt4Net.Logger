using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class ServiceLogResponse
    {
        public DateTime LogTime { get; set; }
        public string Message { get; set; }
        public string Level { get; set; }
    }
}