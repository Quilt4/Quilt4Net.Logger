using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console.Commands.Issue.Type
{
    internal class ListIssueTypesCommand : ActionCommandBase
    {
        private readonly IIssueHandler _issueHandler;

        public ListIssueTypesCommand(IIssueHandler issueHandler)
            : base("List", "List issues")
        {
            _issueHandler = issueHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var projectKey = QueryParam("Project", GetParam(paramList, index++), (await _issueHandler.Client.Actions.Project.GetListAsync()).ToDictionary(x => x.ProjectKey, x => x.Name));
            var applicationKey = QueryParam("Application", GetParam(paramList, index++), (await _issueHandler.Client.Actions.Application.GetListAsync(projectKey)).ToDictionary(x => x.ApplicationKey, x => x.Name));
            var versionKey = QueryParam("Version", GetParam(paramList, index++), (await _issueHandler.Client.Actions.Version.GetListAsync(applicationKey)).ToDictionary(x => x.VersionKey, x => x.VersionNumber));

            var response = await _issueHandler.GetIssueTypesAsync(versionKey);

            var data = new List<string[]> { new[] { "Ticket", "Type" } };
            data.AddRange(response.Select(x => new[] { x.Ticket.ToString(), x.Type.ToString() }));

            OutputTable(data.ToArray());

            return true;
        }
    }
}