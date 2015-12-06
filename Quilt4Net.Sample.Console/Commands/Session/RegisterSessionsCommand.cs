using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class RegisterSessionsCommand : ActionCommandBase
    {
        private readonly Client _client;

        public RegisterSessionsCommand(Client client)
            : base("Register", "Register session")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return !_client.Session.IsRegistered;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var project = await _client.Project.GetAsync(projectKey);
            var environment = QueryParam<string>("Environment", GetParam(paramList, index++));
            var firstAssembly = typeof(RegisterSessionsCommand).GetTypeInfo().Assembly;
            await _client.Session.RegisterAsync(project.ProjectApiKey, environment, firstAssembly);
            return true;
        }
    }
}