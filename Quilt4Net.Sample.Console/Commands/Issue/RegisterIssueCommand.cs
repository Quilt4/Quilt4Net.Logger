using System.Threading.Tasks;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue
{
    internal class RegisterIssueCommand : ActionCommandBase
    {
        private readonly ISessionHandler _sessionHandler;
        private readonly IIssueHandler _issueHandler;

        public RegisterIssueCommand(ISessionHandler sessionHandler, IIssueHandler issueHandler)
            : base("Register", "Register issue")
        {
            _sessionHandler = sessionHandler;
            _issueHandler = issueHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            if (string.IsNullOrEmpty(_sessionHandler.Client.Configuration.ProjectApiKey))
            {
                var index = 0;
                var projectApiKey = QueryParam<string>("ProjectApiKey", GetParam(paramList, index++));
                _sessionHandler.Client.Configuration.ProjectApiKey = projectApiKey;
            }

            var response = await _issueHandler.RegisterAsync("Some warning", MessageIssueLevel.Warning);
            if (response.IsSuccess)
            {
                OutputInformation("Session registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");
                OutputInformation("IssueKey: " + response.Response.IssueKey);
                OutputInformation("IssueTypeUrl: " + response.Response.IssueTypeUrl);
                OutputInformation("IssueUrl: " + response.Response.IssueUrl);
                OutputInformation("Ticket: " + response.Response.Ticket);
            }
            else
            {
                OutputError(response.ErrorMessage + " (" + response.Elapsed.TotalMilliseconds.ToString("0") + "ms)");
            }

            return true;
        }

        private void DoRegisterIssue()
        {
            var response = _issueHandler.Register("Some warning!", MessageIssueLevel.Warning);
            if (response.IsSuccess)
                OutputInformation("Issue registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");
            else
                OutputError(response.ErrorMessage + " (" + response.Elapsed.TotalMilliseconds.ToString("0") + "ms)");
        }
    }
}