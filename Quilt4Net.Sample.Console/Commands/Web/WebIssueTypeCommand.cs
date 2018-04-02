using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Web
{
    internal class WebIssueTypeCommand : ActionCommandBase
    {
        private readonly IWebApiClient _client;
        private readonly IIssueHandler _issueHandler;

        public WebIssueTypeCommand(IIssueHandler issueHandler, IWebApiClient client)
            : base("IssueType", "Simulate the web issue type command")
        {
            _issueHandler = issueHandler;
            _client = client;
        }

        public override void Invoke(string[] param)
        {
            var projectKey = QueryParam("Project", param, _issueHandler.Client.Actions.Project.GetListAsync().Result.ToDictionary(x => x.ProjectKey, x => x.Name));
            var applicationKey = QueryParam("Application", param, _issueHandler.Client.Actions.Application.GetListAsync(projectKey).Result.ToDictionary(x => x.ApplicationKey, x => x.Name));
            var versionKey = QueryParam("Version", param, _issueHandler.Client.Actions.Version.GetListAsync(applicationKey).Result.ToDictionary(x => x.VersionKey, x => x.VersionNumber));
            var issueTypeKey = QueryParam("IssueType", param, _issueHandler.GetIssueTypesAsync(versionKey).Result.ToDictionary(x => x.IssueTypeKey, x => x.Message));
            _client.ReadAsync<object>("project", $"{projectKey}/application/{applicationKey}/version/{versionKey}/issuetype/{issueTypeKey}");
        }
    }
}