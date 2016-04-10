using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Web
{
    internal class WebCommands : ContainerCommandBase
    {
        public WebCommands(IIssueHandler issueHandler, IWebApiClient client)
            : base("Web")
        {
            RegisterCommand(new WebIssueTypeCommand(issueHandler, client));
        }
    }
}