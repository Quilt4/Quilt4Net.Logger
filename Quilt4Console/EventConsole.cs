using Quilt4Console.Commands;
using Quilt4Net;
using Quilt4Net.Core.Events;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console
{
    internal class EventConsole
    {
        private readonly ProgramRootCommand _rootCommand;

        public EventConsole(ProgramRootCommand rootCommand, Configuration configuration, Quilt4Client client, SessionHandler sessionHandler, IssueHandler issueHandler)
        {
            _rootCommand = rootCommand;

            sessionHandler.SessionRegistrationStartedEvent += Session_SessionRegistrationStartedEvent;
            sessionHandler.SessionRegistrationCompletedEvent += SessionHandler_SessionRegistrationCompletedEvent;
            sessionHandler.SessionEndStartedEvent += Session_SessionEndStartedEvent;
            sessionHandler.SessionEndCompletedEvent += Session_SessionEndCompletedEvent;
            issueHandler.IssueRegistrationStartedEvent += Issue_IssueRegistrationStartedEvent;
            issueHandler.IssueRegistrationCompletedEvent += Issue_IssueRegistrationCompletedEvent;
            //client.Client.AuthorizationChangedEvent += WebApiClient_AuthorizationChangedEvent;
            //client.Client.WebApiRequestEvent += WebApiClient_WebApiRequestEvent;
            //client.Client.WebApiResponseEvent += WebApiClient_WebApiResponseEvent;
        }

        private void Session_SessionRegistrationStartedEvent(object sender, SessionRegistrationStartedEventArgs e)
        {
            _rootCommand.OutputEvent("Starting to register session.");
        }

        private void SessionHandler_SessionRegistrationCompletedEvent(object sender, SessionRegistrationCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                var message = $"Session {e.Result.ErrorMessage ?? "registered in"} {e.Result.Elapsed.TotalMilliseconds.ToString("0")}ms.";
                _rootCommand.OutputEvent(message);
            }
            else
            {
                _rootCommand.OutputError(e.Result.Exception);
            }
        }

        private void Session_SessionEndStartedEvent(object sender, SessionEndStartedEventArgs e)
        {
            _rootCommand.OutputEvent("Starting to end session.");
        }

        private void Session_SessionEndCompletedEvent(object sender, SessionEndCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                var message = $"Session {e.Result.ErrorMessage ?? "ended in"} {e.Result.Elapsed.TotalMilliseconds.ToString("0")}ms.";
                _rootCommand.OutputEvent(message);
            }
            else
            {
                _rootCommand.OutputError(e.Result.Exception);
            }
        }

        private void Issue_IssueRegistrationStartedEvent(object sender, IssueRegistrationStartedEventArgs e)
        {
            _rootCommand.OutputEvent("Starting to register issue.");
        }

        private void Issue_IssueRegistrationCompletedEvent(object sender, IssueRegistrationCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                var message = $"Issue {e.Result.ErrorMessage ?? "registered in"} {e.Result.Elapsed.TotalMilliseconds.ToString("0")}ms.";
                _rootCommand.OutputEvent(message);
            }
            else
            {
                _rootCommand.OutputError(e.Result.Exception);
            }
        }

        private void WebApiClient_AuthorizationChangedEvent(object sender, AuthorizationChangedEventArgs e)
        {
            _rootCommand.OutputEvent($"Authorization changed to {(e.IsAuthorized ? "authorized" : "unauthorized")}.");
        }

        private void WebApiClient_WebApiRequestEvent(object sender, Quilt4Net.Core.Interfaces.WebApiRequestEventArgs e)
        {
            _rootCommand.OutputEvent("Sending WebAPI call to " + e.Path + ".");
        }

        private void WebApiClient_WebApiResponseEvent(object sender, Quilt4Net.Core.Interfaces.WebApiResponseEventArgs e)
        {
            if (e.IsSuccess)
            {
                _rootCommand.OutputEvent("Got response from WebAPI call after {0} sec.", OutputLevel.Default, e.Elapsed.TotalSeconds);
            }
            else
            {
                _rootCommand.OutputError(e.Exception);
            }
        }
    }
}