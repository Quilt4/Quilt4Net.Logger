using System;
using System.Collections.Generic;

namespace Quilt4Net.Core.DataTransfer
{
    public class IssueData
    {
        internal IssueData()
        {
        }

        public string ProjectApiKey { get; set; }
        public Guid IssueKey { get; set; }
        public Guid SessionKey { get; set; }
        public DateTime ClientTime { get; set; }
        public IDictionary<string, string> Data { get; set; }
        public IssueTypeData IssueType { get; set; }
        public Guid? IssueThreadKey { get; set; }
        public string UserHandle { get; set; }
    }
}