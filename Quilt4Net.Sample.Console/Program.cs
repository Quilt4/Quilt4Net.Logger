using Quilt4Net.Core.Events;
using Quilt4Net.Handlers;
using Quilt4Net.Sample.Console.Commands.Invitation;
using Quilt4Net.Sample.Console.Commands.Issue;
using Quilt4Net.Sample.Console.Commands.Project;
using Quilt4Net.Sample.Console.Commands.Service;
using Quilt4Net.Sample.Console.Commands.Session;
using Quilt4Net.Sample.Console.Commands.Setting;
using Quilt4Net.Sample.Console.Commands.User;
using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console
{
    static class Program
    {
        private static RootCommand _rootCommand;

        static void Main(string[] args)
        {
            var console = new ClientConsole();

            //Note: Using the single instance version
            //var configuration = Configuration.Instance;
            //var client = Quilt4Net.Quilt4NetClient.Instance;

            //Note: Using the created instance version
            var configuration = new ConfigurationHandler();
            var client = new Quilt4NetClient(configuration);

            //Note: Config in code
            configuration.ProjectApiKey = "C9DTTXV7T0ELMBKAGSO26LFIGMUOEBMX";
            configuration.UseBuildTime = true;
            configuration.Target.Location = "http://localhost:29660";
            configuration.Session.Environment = "Manual";

            client.SessionHandler.SessionRegistrationStartedEvent += Session_SessionRegistrationStartedEvent;
            client.SessionHandler.SessionRegistrationCompletedEvent += SessionSessionRegistrationCompletedEvent;
            client.SessionHandler.SessionEndStartedEvent += Session_SessionEndStartedEvent;
            client.SessionHandler.SessionEndCompletedEvent += Session_SessionEndCompletedEvent;
            client.IssueHandler.IssueRegistrationStartedEvent += Issue_IssueRegistrationStartedEvent;
            client.IssueHandler.IssueRegistrationCompletedEvent += Issue_IssueRegistrationCompletedEvent;
            client.WebApiClient.AuthorizationChangedEvent += WebApiClient_AuthorizationChangedEvent;

            _rootCommand = new RootCommand(console);
            _rootCommand.RegisterCommand(new UserCommands(client));
            _rootCommand.RegisterCommand(new ProjectCommands(client));
            _rootCommand.RegisterCommand(new InvitationCommands(client));
            _rootCommand.RegisterCommand(new SessionCommands(client));
            _rootCommand.RegisterCommand(new IssueCommands(client));
            _rootCommand.RegisterCommand(new SettingCommands(client));
            _rootCommand.RegisterCommand(new ServiceCommands(client));
            new CommandEngine(_rootCommand).Run(args);

            client.Dispose();
        }

        private static void Session_SessionRegistrationStartedEvent(object sender, SessionRegistrationStartedEventArgs e)
        {
            _rootCommand.OutputEvent("Starting to register session.");
        }

        private static void SessionSessionRegistrationCompletedEvent(object sender, SessionRegistrationCompletedEventArgs e)
        {
            var message = $"Session {e.Result.ErrorMessage ?? "registered in"} {e.Result.Elapsed.TotalMilliseconds.ToString("0")}ms.";
            _rootCommand.OutputEvent(message);
        }

        private static void Session_SessionEndStartedEvent(object sender, SessionEndStartedEventArgs e)
        {
            _rootCommand.OutputEvent("Starting to end session.");
        }

        private static void Session_SessionEndCompletedEvent(object sender, SessionEndCompletedEventArgs e)
        {
            var message = $"Session {e.Result.ErrorMessage ?? "ended in"} {e.Result.Elapsed.TotalMilliseconds.ToString("0")}ms.";
            _rootCommand.OutputEvent(message);
        }

        private static void Issue_IssueRegistrationStartedEvent(object sender, IssueRegistrationStartedEventArgs e)
        {
            _rootCommand.OutputEvent("Starting to register issue.");
        }

        private static void Issue_IssueRegistrationCompletedEvent(object sender, IssueRegistrationCompletedEventArgs e)
        {
            var message = $"Issue {e.Result.ErrorMessage ?? "registered in"} {e.Result.Elapsed.TotalMilliseconds.ToString("0")}ms.";
            _rootCommand.OutputEvent(message);
        }

        private static void WebApiClient_AuthorizationChangedEvent(object sender, AuthorizationChangedEventArgs e)
        {
            _rootCommand.OutputEvent($"Authorization changed to {(e.IsAuthorized ? "authorized" : "unauthorized")}.");
        }
    }
}