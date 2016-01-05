using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class RegisterSessionCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public RegisterSessionCommand(IQuilt4NetClient client)
            : base("Register", "Register session")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            //if (!_client.User.IsAuthorized)
            //    return false;

            return !_client.SessionHandler.IsRegistered;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            //var index = 0;
            //var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            //var project = await _client.Project.GetAsync(projectKey);
            //var environment = QueryParam<string>("Environment", GetParam(paramList, index++));
            //OutputInformation("ProjectApiKey: {0}", project.ProjectApiKey);
            //_client.Configuration.ProjectApiKey = project.ProjectApiKey;
            //_client.Configuration.Session.Environment = environment;

            //_client.Session.RegisterStart();
            var response = await _client.SessionHandler.RegisterAsync(Assembly.GetExecutingAssembly());
            if (response.IsSuccess)
                OutputInformation("Session registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");
            else
                OutputError(response.ErrorMessage + " (" + response.Elapsed.TotalMilliseconds.ToString("0") + "ms)");

            return true;
        }
    }
}