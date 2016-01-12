using System.Threading.Tasks;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Issue
{
    internal class RegisterIssueCommand : ActionCommandBase
    {
        private readonly IIssueHandler _client;

        public RegisterIssueCommand(IIssueHandler client)
            : base("Register", "Register issue")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var response = await _client.RegisterAsync("Some warning.", MessageIssueLevel.Warning);
            if (response.IsSuccess)
                OutputInformation("Issue registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");
            else
                OutputError(response.ErrorMessage + " (" + response.Elapsed.TotalMilliseconds.ToString("0") + "ms)");

            return true;
        }
    }
}