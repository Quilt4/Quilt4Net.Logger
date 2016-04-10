using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Web
{
    internal class WebIssueTypeCommand : ActionCommandBase
    {
        private readonly IIssueHandler _issueHandler;
        private readonly IWebApiClient _client;

        public WebIssueTypeCommand(IIssueHandler issueHandler, IWebApiClient client)
            : base("IssueType", "Simulate the web issue type command")
        {
            _issueHandler = issueHandler;
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _issueHandler.Client.Actions.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var applicationKey = QueryParam("Application", GetParam(paramList, index++), (await _issueHandler.Client.Actions.Application.GetListAsync(projectKey)).ToDictionary(x => x.ApplicationKey, x => x.Name));
            var versionKey = QueryParam("Version", GetParam(paramList, index++), (await _issueHandler.Client.Actions.Version.GetListAsync(applicationKey)).ToDictionary(x => x.VersionKey, x => x.VersionNumber));
            var issueTypeKey = QueryParam("IssueType", GetParam(paramList, index++), (await _issueHandler.GetIssueTypesAsync(versionKey)).ToDictionary(x => x.IssueTypeKey, x => x.Message));

            var result = await _client.ReadAsync<object>("project", string.Format("{0}/application/{1}/version/{2}/issuetype/{3}", projectKey, applicationKey, versionKey, issueTypeKey));

            return true;
        }
    }
}
