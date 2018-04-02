using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue
{
    internal class ListIssueCommand : ActionCommandBase
    {
        private readonly IIssueHandler _issueHandler;

        public ListIssueCommand(IIssueHandler issueHandler)
            : base("List", "List issues.")
        {
            _issueHandler = issueHandler;
        }

        public override void Invoke(string[] param)
        {
            var projectKey = QueryParam("Project", param, _issueHandler.Client.Actions.Project.GetListAsync().Result.ToDictionary(x => x.ProjectKey, x => x.Name));
            var applicationKey = QueryParam("Application", param, _issueHandler.Client.Actions.Application.GetListAsync(projectKey).Result.ToDictionary(x => x.ApplicationKey, x => x.Name));
            var versionKey = QueryParam("Version", param, _issueHandler.Client.Actions.Version.GetListAsync(applicationKey).Result.ToDictionary(x => x.VersionKey, x => x.VersionNumber));

            var response = _issueHandler.GetIssuesAsync(versionKey).Result;
            foreach (var item in response) OutputInformation($"{item.IssueKey}\t{item.Ticket}");
        }
    }
}