using System;
using System.Collections.Generic;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.DataTransfer
{
    public class IssueRequest : IIssueData
    {
        internal IssueRequest()
        {
        }

        public Guid IssueKey { get; set; }
        public string SessionToken { get; set; }
        public DateTime ClientTime { get; set; }
        public IDictionary<string, string> Data { get; set; }
        public IssueTypeData IssueType { get; set; }
        public Guid? IssueThreadKey { get; set; }
        public string UserHandle { get; set; }
    }
}