using System.Collections.Generic;

namespace Quilt4Net.Core.DataTransfer
{
    public class IssueTypeData
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Type { get; set; }
        public IssueTypeData[] InnerIssueTypes { get; set; }
        public IDictionary<string, string> Data { get; set; }
    }
}