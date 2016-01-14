using System;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal static class IssueLevelExtension
    {
        internal static IssueLevel ToIssueLevel(this ExceptionIssueLevel issueLevel, IConfiguration configuration)
        {
            IssueLevel il;
            if (!Enum.TryParse(issueLevel.ToString(), true, out il)) throw new ExpectedIssues(configuration).GetException(ExpectedIssues.CannotParseIssueLevelException).AddData("IssueLevel", issueLevel);
            return il;
        }

        internal static IssueLevel ToIssueLevel(this MessageIssueLevel issueLevel, IConfiguration configuration)
        {
            IssueLevel il;
            if (!Enum.TryParse(issueLevel.ToString(), true, out il)) throw new ExpectedIssues(configuration).GetException(ExpectedIssues.CannotParseIssueLevelMessage).AddData("issueLevel", issueLevel);
            return il;
        }
    }
}