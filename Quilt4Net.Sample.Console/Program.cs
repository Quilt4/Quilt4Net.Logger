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
        private static ClientConsole _console;

        static void Main(string[] args)
        {
            //Quilt4Net.Session.Instance.SessionRegisteredEvent += Instance_SessionRegisteredEvent;

            _console = new ClientConsole();

            //var port = 29660;
            //var address = new Uri($"http://localhost:{port}/");
            //var client = new Client(new WebApiClient(address, new TimeSpan(0, 0, 0, 30)));
            //var configuration = new Configuration();
            //var client = new Client(Configuration.Instance);

            Configuration.Instance.ProjectApiKey = "BL2VV8LVF0C9GWRTX6CS03R7IK1PYT7E";
            Configuration.Instance.Target.Location = "http://localhost:29660";
            Configuration.Instance.Session.Environment = "Manual Test";
            var client = Client.Instance;

            client.Session.SessionRegistrationStartedEvent += Session_SessionRegistrationStartedEvent;
            client.Session.SessionRegistrationCompletedEvent += SessionSessionRegistrationCompletedEvent;
            client.Issue.IssueRegistrationStartedEvent += Issue_IssueRegistrationStartedEvent;
            client.Issue.IssueRegistrationCompletedEvent += Issue_IssueRegistrationCompletedEvent;

            //_console.WriteLine("Using serer " + address, OutputLevel.Information, null);

            var rootCommand = new RootCommand(_console);
            rootCommand.RegisterCommand(new UserCommands(client));
            rootCommand.RegisterCommand(new ProjectCommands(client));
            rootCommand.RegisterCommand(new SessionCommands(client));
            rootCommand.RegisterCommand(new IssueCommands(client));
            new CommandEngine(rootCommand).Run(args);

            client.Dispose();
        }

        private static void Session_SessionRegistrationStartedEvent(object sender, SessionRegistrationStartedEventArgs e)
        {
            _console.WriteLine("Starting to register session.", OutputLevel.Information, ConsoleColor.DarkCyan);
        }

        private static void SessionSessionRegistrationCompletedEvent(object sender, SessionRegistrationCompletedEventArgs e)
        {
            var message = $"Session {e.Result.ErrorMessage ?? "registered in "}{e.Result.Elapsed.TotalMilliseconds.ToString("0")}ms.";
            var outputLevel = e.Result.IsSuccess ? OutputLevel.Information : OutputLevel.Error;
            //TODO: Have a way of outputing an exception using Tharga Console
            _console.WriteLine(message, outputLevel, ConsoleColor.DarkCyan);
        }

        private static void Issue_IssueRegistrationStartedEvent(object sender, IssueRegistrationStartedEventArgs e)
        {
            _console.WriteLine("Starting to register issue.", OutputLevel.Information, ConsoleColor.DarkCyan);
        }

        private static void Issue_IssueRegistrationCompletedEvent(object sender, IssueRegistrationCompletedEventArgs e)
        {
            var message = $"Issue {e.Result.ErrorMessage ?? "registered in "}{e.Result.Elapsed.TotalMilliseconds.ToString("0")}ms.";
            var outputLevel = e.Result.IsSuccess ? OutputLevel.Information : OutputLevel.Error;
            //TODO: Have a way of outputing an exception using Tharga Console
            _console.WriteLine(message, outputLevel, ConsoleColor.DarkCyan);
        }
    }
}