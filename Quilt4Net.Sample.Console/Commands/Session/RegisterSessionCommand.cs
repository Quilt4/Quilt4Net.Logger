using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.Session
{
    internal class RegisterSessionCommand : ActionCommandBase
    {
        private readonly ISessionHandler _sessionHandler;

        public RegisterSessionCommand(ISessionHandler sessionHandler)
            : base("Register", "Register session")
        {
            _sessionHandler = sessionHandler;
        }

        public override bool CanExecute()
        {
            //if (!_client.User.IsAuthorized)
            //    return false;

            return !_sessionHandler.IsRegisteredOnServer;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            if (string.IsNullOrEmpty(_sessionHandler.Client.Configuration.ProjectApiKey))
            {
                var index = 0;
                var projectApiKey = QueryParam<string>("ProjectApiKey", GetParam(paramList, index++));
                _sessionHandler.Client.Configuration.ProjectApiKey = projectApiKey;
            }

            var response = await _sessionHandler.RegisterAsync(Assembly.GetExecutingAssembly());
            if (response.IsSuccess)
                OutputInformation("Session registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");
            else
                OutputError(response.ErrorMessage + " (" + response.Elapsed.TotalMilliseconds.ToString("0") + "ms)");

            return true;
        }
    }
}