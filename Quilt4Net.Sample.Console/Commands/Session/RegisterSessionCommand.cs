using System.Reflection;
using Quilt4Net.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

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

        public override bool CanExecute(out string reasonMessage)
        {
            reasonMessage = string.Empty;
            if (_sessionHandler.IsRegisteredOnServer)
                reasonMessage = "Registered on server";
            return !_sessionHandler.IsRegisteredOnServer;
        }

        public override void Invoke(string[] param)
        {
            if (string.IsNullOrEmpty(_sessionHandler.Client.Configuration.ProjectApiKey))
            {
                var index = 0;
                var projectApiKey = QueryParam<string>("ProjectApiKey", param);
                _sessionHandler.Client.Configuration.ProjectApiKey = projectApiKey;
            }

            var response = _sessionHandler.RegisterAsync(Assembly.GetExecutingAssembly()).Result;
            if (response.IsSuccess)
            {
                OutputInformation("Session registration took " + response.Elapsed.TotalMilliseconds.ToString("0") + "ms.");
                OutputInformation("Session Url: " + response.Response.SessionUrl);
            }
            else
            {
                OutputError(response.ErrorMessage + " (" + response.Elapsed.TotalMilliseconds.ToString("0") + "ms)");
            }

        }
    }
}