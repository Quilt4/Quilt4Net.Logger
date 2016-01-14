using System;

namespace Quilt4Net.Core.DataTransfer
{
    public class IssueResponse
    {
        public Guid IssueKey { get; set; }
        public DateTime ServerTime { get; set; }
        public string Ticket { get; set; }
        public string IssueTypeUrl { get; set; }
        public string IssueUrl { get; set; }
    }
}