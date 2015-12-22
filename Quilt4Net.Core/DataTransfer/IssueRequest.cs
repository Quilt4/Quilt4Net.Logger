using System;
using System.Collections.Generic;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.DataTransfer
{
    public class IssueResponse : IIssueData
    {
        internal IssueResponse()
        {
        }

        public Guid IssueKey { get; internal set; }
        public Guid SessionKey { get; internal set; }
        public DateTime ClientTime { get; internal set; }
        public IDictionary<string, string> Data { get; internal set; }
        public IssueTypeData IssueType { get; internal set; }
        public Guid? IssueThreadKey { get; internal set; }
        public string UserHandle { get; internal set; }
        public DateTime ServerTime { get; internal set; }
        public string Ticket { get; internal set; }
    }

    public class IssueRequest : IIssueData
    {
        internal IssueRequest()
        {
        }

        public string ProjectApiKey { get; internal set; }
        public Guid IssueKey { get; internal set; }
        public Guid SessionKey { get; internal set; }
        public DateTime ClientTime { get; internal set; }
        public IDictionary<string, string> Data { get; internal set; }
        public IssueTypeData IssueType { get; internal set; }
        public Guid? IssueThreadKey { get; internal set; }
        public string UserHandle { get; internal set; }
    }
}