using System;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Exceptions;
using Quilt4Net.Core.Handlers;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal static class IssueLevelExtension
    {
        internal static IssueLevel ToIssueLevel(this IssueHandlerBase.ExceptionIssueLevel issueLevel, IConfigurationHandler configurationHandler)
        {
            IssueLevel il;
            if (!Enum.TryParse(issueLevel.ToString(), true, out il)) throw new ExpectedIssues(configurationHandler).GetException(ExpectedIssues.CannotParseIssueLevelException).AddData("IssueLevel", issueLevel);

            return il;
        }

        internal static IssueLevel ToIssueLevel(this IssueHandlerBase.MessageIssueLevel issueLevel, IConfigurationHandler configurationHandler)
        {
            IssueLevel il;
            if (!Enum.TryParse(issueLevel.ToString(), true, out il)) throw new ExpectedIssues(configurationHandler).GetException(ExpectedIssues.CannotParseIssueLevelMessage).AddData("issueLevel", issueLevel);

            return il;
        }
    }
}