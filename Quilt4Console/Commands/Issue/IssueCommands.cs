using Quilt4Console.Commands.Issue.Type;
using Quilt4Net.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Issue
{
    internal class IssueCommands : ContainerCommandBase
    {
        public IssueCommands(ISessionHandler sessionHandler, IIssueHandler issueHandler)
            : base("Issue")
        {
            //RegisterCommand(new RegisterIssueCommand(sessionHandler, issueHandler));
            RegisterCommand(new ListIssueCommand(issueHandler));
            RegisterCommand(new IssueTypeCommands(issueHandler));
        }
    }
}