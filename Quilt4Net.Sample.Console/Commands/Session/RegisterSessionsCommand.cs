using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core;
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

        //public override bool CanExecute()
        //{
        //    if (!_client.User.IsAuthorized)
        //        return false;

        //    return !_client.Session.IsRegistered;
        //}

        public override async Task<bool> InvokeAsync(string paramList)
        {
            //var index = 0;
            //var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            //var project = await _client.Project.GetAsync(projectKey);
            //var environment = QueryParam<string>("Environment", GetParam(paramList, index++));
            //OutputInformation("ProjectApiKey: {0}", project.ProjectApiKey);
            //_client.Configuration.ProjectApiKey = project.ProjectApiKey;
            //_client.Configuration.Session.Environment = environment;

            _client.Configuration.ProjectApiKey = "BL2VV8LVF0C9GWRTX6CS03R7IK1PYT7E";

            //await _client.Session.RegisterAsync();

            _client.Session.RegisterStart();

            //var response = _client.Session.Register();
            //OutputInformation(response.Elapsed.TotalMilliseconds.ToString("0") + "ms");

            return true;
        }
    }
}