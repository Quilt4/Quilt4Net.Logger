using Quilt4Net.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Project
{
    internal class ProjectCommands : ContainerCommandBase
    {
        public ProjectCommands(IQuilt4NetClient client)
            : base("Project")
        {
            RegisterCommand(new CreateProjectCommand(client));
            RegisterCommand(new ListProjectsCommand(client));
            RegisterCommand(new GetProjectCommand(client));
            RegisterCommand(new UpdateProjectCommand(client));
            RegisterCommand(new DeleteProjectCommand(client));
            RegisterCommand(new ListMemberProjectsCommand(client));
        }
    }
}