using System.Collections.Generic;
using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue.Type
{
    internal class ListIssueTypesCommand : ActionCommandBase
    {
        private readonly IIssueHandler _issueHandler;

        public ListIssueTypesCommand(IIssueHandler issueHandler)
            : base("List", "List issues")
        {
            _issueHandler = issueHandler;
        }

        public override void Invoke(string[] param)
        {
            var projectKey = QueryParam("Project", param, (_issueHandler.Client.Actions.Project.GetListAsync().Result).ToDictionary(x => x.ProjectKey, x => x.Name));
            var applicationKey = QueryParam("Application", param, (_issueHandler.Client.Actions.Application.GetListAsync(projectKey).Result).ToDictionary(x => x.ApplicationKey, x => x.Name));
            var versionKey = QueryParam("Version", param, (_issueHandler.Client.Actions.Version.GetListAsync(applicationKey).Result).ToDictionary(x => x.VersionKey, x => x.VersionNumber));

            var response = _issueHandler.GetIssueTypesAsync(versionKey).Result;

            var data = new List<string[]> { new[] { "Ticket", "Type" } };
            data.AddRange(response.Select(x => new[] { x.Ticket.ToString(), x.Type.ToString() }));

            OutputTable(data.ToArray());
        }
    }
}