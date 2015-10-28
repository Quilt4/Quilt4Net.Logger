using System;
using System.Collections.Generic;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Issue : IIssue
    {
        public Issue(IIssueType issueType, ISession session, IssueResponse issueResponse)
        {
            IssueTime = issueResponse.IssueTime;
            //LinkedIssues { get; } //TODO: Handle
            Data = issueResponse.Data;
            UserInput = issueResponse.UserInput;
            Visible = issueResponse.Visible;
            Session = session;
            IssueType = issueType;
        }

        public DateTime IssueTime { get; }
        public IIssue LinkedIssues { get { throw new NotImplementedException(); } }
        public IDictionary<string, string> Data { get; }
        public string UserInput { get; }
        public bool? Visible { get; }
        public ISession Session { get; }
        public IIssueType IssueType { get; }
    }
}