using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue.Type
{
    internal class IssueTypeCommands : ContainerCommandBase
    {
        public IssueTypeCommands(IIssueHandler issueHandler)
            : base("Type")
        {
            RegisterCommand(new ListIssueTypesCommand(issueHandler));
        }
    }
}