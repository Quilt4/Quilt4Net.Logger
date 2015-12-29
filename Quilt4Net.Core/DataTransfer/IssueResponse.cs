using System;
using System.Collections.Generic;
using Quilt4Net.Core.Interfaces;

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

    public class IssueResponse : IIssueData
    {
        public Guid IssueKey { get; set; }
        public Guid SessionKey { get; set; }
        public DateTime ClientTime { get; set; }
        public IDictionary<string, string> Data { get; set; }
        public IssueTypeData IssueType { get; set; }
        public Guid? IssueThreadKey { get; set; }
        public string UserHandle { get; set; }
        public DateTime ServerTime { get; set; }
        public string Ticket { get; set; }
        public string IssueUrl { get; set; }
    }
}