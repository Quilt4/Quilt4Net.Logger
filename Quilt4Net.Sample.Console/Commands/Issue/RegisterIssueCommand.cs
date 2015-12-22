using System.Threading.Tasks;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue
{
    internal class RegisterIssueCommand : ActionCommandBase
    {
        private readonly Client _client;

        public RegisterIssueCommand(Client client)
            : base("Register", "Register issue")
        {
            _client = client;
        }

        public override bool CanExecute()
        {
            return !_client.Session.IsRegistered;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.Issue.RegisterAsync("Some warning.", Core.Issue.MessageIssueLevel.Warning);
            OutputEvent("Issue registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");

            //var index = 0;
            //var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _client.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            //var project = await _client.Project.GetAsync(projectKey);
            //var environment = QueryParam<string>("Environment", GetParam(paramList, index++));
            //await _client.Session.RegisterAsync(); //project.ProjectApiKey, environment);

            //var result = await _client.Issue.RegisterAsync();

            return true;
        }
    }
}