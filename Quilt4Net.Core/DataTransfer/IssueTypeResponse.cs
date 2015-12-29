using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class IssueTypeResponse
    {
        public Guid IssueTypeKey { get; set; }
        public Guid VersionKey { get; set; }
        public string Type { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int Ticket { get; set; }
        public DateTime CreationServerTime { get; set; }
        public DateTime LastIssueServerTime { get; set; }
    }
}