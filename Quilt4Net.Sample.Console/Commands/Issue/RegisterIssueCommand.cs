using System.Threading.Tasks;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue
{
    internal class RegisterIssueCommand : ActionCommandBase
    {
        private readonly IIssueHandler _issueHandler;

        public RegisterIssueCommand(IIssueHandler issueHandler)
            : base("Register", "Register issue")
        {
            _issueHandler = issueHandler;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            Task.Run(() => DoRegisterIssue());
            Task.Run(() => DoRegisterIssue());
            Task.Run(() => DoRegisterIssue());
            Task.Run(() => DoRegisterIssue());
            Task.Run(() => DoRegisterIssue());

            return true;
        }

        private void DoRegisterIssue()
        {
            var response = _issueHandler.Register("Some warning.", MessageIssueLevel.Warning);
            if (response.IsSuccess)
                OutputInformation("Issue registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");
            else
                OutputError(response.ErrorMessage + " (" + response.Elapsed.TotalMilliseconds.ToString("0") + "ms)");
        }
    }
}