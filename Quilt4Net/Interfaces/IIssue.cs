using System;
using System.Collections.Generic;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IIssue
    {
        DateTime IssueTime { get; }
        IIssue LinkedIssues { get; }
        IDictionary<string, string> Data { get; }
        string UserInput { get; }
        bool? Visible { get; }
        ISession Session { get; }

        //Up-links
        IIssueType IssueType { get; }
    }
}