using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue
{
    internal class IssueCommands : ContainerCommandBase
    {
        public IssueCommands(Client client)
            : base("Issue")
        {
            RegisterCommand(new RegisterIssueCommand(client));
        }
    }
}