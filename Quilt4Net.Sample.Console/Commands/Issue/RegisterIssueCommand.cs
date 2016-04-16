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
            _issueHandler.RegisterStart("A");
            _issueHandler.RegisterStart("B");
            _issueHandler.RegisterStart("C");
            //var response = await _issueHandler.RegisterAsync("Some warning.", MessageIssueLevel.Warning);
            await _issueHandler.RegisterAsync("Some warning.", MessageIssueLevel.Warning);
            _issueHandler.RegisterStart("D");
            _issueHandler.RegisterStart("E");
            var response = _issueHandler.Register("Some warning.", MessageIssueLevel.Warning);
            _issueHandler.RegisterStart("F");
            _issueHandler.RegisterStart("G");
            if (response.IsSuccess)
                OutputInformation("Issue registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");
            else
                OutputError(response.ErrorMessage + " (" + response.Elapsed.TotalMilliseconds.ToString("0") + "ms)");

            return true;
        }
    }
}