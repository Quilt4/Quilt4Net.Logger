using Quilt4Net.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Project
{
    internal class ProjectCommands : ContainerCommandBase
    {
        public ProjectCommands(IQuilt4NetClient client)
            : base("Project")
        {
            RegisterCommand(new ListProjectsCommand(client));
        }
    }
}