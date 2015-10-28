using System;
using System.Collections.Generic;

namespace Tharga.Quilt4Net.DataTransfer
{
    public class IssueResponse
    {
        public string ApplicationName { get; set; }
        public string VersionName { get; set; }
        public DateTime IssueTime { get; set; }
        public IDictionary<string, string> Data { get; set; }
        public string UserInput { get; set; }
        public bool? Visible { get; set; }
        public string IssueTypeMessage { get; set; }
        public Guid SessionKey { get; set; }
    }
}