using Tharga.Quilt4Net;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class ProjectCommands : ContainerCommandBase
    {
        public ProjectCommands(Client client)
            : base("Project")
        {
            RegisterCommand(new ListProjectCommand(client));
            RegisterCommand(new DeleteProjectCommand(client));
        }
    }
}