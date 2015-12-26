using System;
using System.Collections.Generic;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.DataTransfer
{
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