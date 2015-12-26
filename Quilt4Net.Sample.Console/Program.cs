using System;
using Quilt4Net.Core.Events;
using Quilt4Net.Sample.Console.Commands.Issue;
using Quilt4Net.Sample.Console.Commands.Project;
using Quilt4Net.Sample.Console.Commands.Session;
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
            //var client = Client.Instance;

            //Note: Using the created instance version
            var configuration = new Configuration();
            var client = new Client(configuration);

            configuration.ProjectApiKey = "BL2VV8LVF0C9GWRTX6CS03R7IK1PYT7E";
            configuration.Target.Location = "http://localhost:29660";
            configuration.Session.Environment = "Manual";

            client.Session.SessionRegistrationStartedEvent += Session_SessionRegistrationStartedEvent;
            client.Session.SessionRegistrationCompletedEvent += SessionSessionRegistrationCompletedEvent;
            client.Session.SessionEndStartedEvent += Session_SessionEndStartedEvent;
            client.Session.SessionEndCompletedEvent += Session_SessionEndCompletedEvent;
            client.Issue.IssueRegistrationStartedEvent += Issue_IssueRegistrationStartedEvent;
            client.Issue.IssueRegistrationCompletedEvent += Issue_IssueRegistrationCompletedEvent;

            _rootCommand = new RootCommand(console);
            _rootCommand.RegisterCommand(new UserCommands(client));
            _rootCommand.RegisterCommand(new ProjectCommands(client));
            _rootCommand.RegisterCommand(new SessionCommands(client));
            _rootCommand.RegisterCommand(new IssueCommands(client));
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
    }
}