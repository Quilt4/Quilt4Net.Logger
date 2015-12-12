using System.Linq;
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
            if (!_client.User.IsAuthorized)
                return false;

            return !_client.Session.IsRegistered;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var project = await _client.Project.GetAsync(projectKey);
            var environment = QueryParam<string>("Environment", GetParam(paramList, index++));
            _client.Configuration.ProjectApiKey = project.ProjectApiKey;
            _client.Configuration.Session.Environment = environment;
            await _client.Session.RegisterAsync();
            return true;
        }
    }
}