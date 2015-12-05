using Tharga.Quilt4Net;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Project
{
    internal class ProjectCommands : ContainerCommandBase
    {
        public ProjectCommands(Client client)
            : base("Project")
        {
            RegisterCommand(new CreateProjectCommand(client));
            RegisterCommand(new ListProjectsCommand(client));
            RegisterCommand(new GetProjectCommand(client));
            RegisterCommand(new UpdateProjectCommand(client));
            RegisterCommand(new DeleteProjectCommand(client));
        }
    }
}