using Quilt4Net.Core.Interfaces;
using Quilt4Net.Sample.Console.Commands.Issue.Type;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue
{
    internal class IssueCommands : ContainerCommandBase
    {
        public IssueCommands(ISessionHandler sessionHandler, IIssueHandler issueHandler)
            : base("Issue")
        {
            RegisterCommand(new RegisterIssueCommand(sessionHandler, issueHandler));
            RegisterCommand(new ListIssueCommand(issueHandler));
            RegisterCommand(new IssueTypeCommands(issueHandler));
        }
    }
}