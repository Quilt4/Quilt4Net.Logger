using System;
using System.Collections.Generic;

namespace Quilt4Net.Core.DataTransfer
{
    public enum IssueLevel
    {
        Information,
        Warning,
        Error,
    }

    public class IssueRequest
    {
        public string ProjectApiKey { get; set; }
        public Guid IssueKey { get; set; }
        public Guid SessionKey { get; set; }
        public DateTime ClientTime { get; set; }
        public IDictionary<string, string> Data { get; set; }
        public IssueTypeData IssueType { get; set; }
        public Guid? IssueThreadKey { get; set; }
        public string UserHandle { get; set; }
    }

    public class IssueTypeData
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public IssueLevel IssueLevel { get; set; }
        public string Type { get; set; }
        public IssueTypeData Inner { get; set; }
    }

    public class SessionRequest
    {
        internal SessionRequest()
        {
        }

        public Guid SessionKey { get; set; }
        public string ProjectApiKey { get; set; }
        public DateTime ClientStartTime { get; set; }
        public string Environment { get; set; }
        public ApplicationData Application { get; set; }
        public MachineData Machine { get; set; }
        public UserData User { get; set; }
    }
}