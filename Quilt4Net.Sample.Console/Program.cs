using System;
using Quilt4Net.Core;
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
            _console = new ClientConsole();

            var port = 29660;
            var address = new Uri($"http://localhost:{port}/");
            var client = new Client(new WebApiClient(address, new TimeSpan(0, 0, 0, 30)));
            client.Session.SessionRegisteredEvent += Session_SessionRegisteredEvent;

            _console.WriteLine("Using serer " + address, OutputLevel.Information, null);

            var rootCommand = new RootCommand(_console);
            rootCommand.RegisterCommand(new UserCommands(client));
            rootCommand.RegisterCommand(new ProjectCommands(client));
            rootCommand.RegisterCommand(new SessionCommands(client));
            rootCommand.RegisterCommand(new IssueCommands(client));
            new CommandEngine(rootCommand).Run(args);
        }

        private static void Session_SessionRegisteredEvent(object sender, SessionRegisteredEventArgs e)
        {
            var message = string.Format("{0} {1}ms", e.ErrorMessage ?? "OK.", e.Elapsed.TotalMilliseconds.ToString("0"));
            var outputLevel = e.IsSuccess ? OutputLevel.Information : OutputLevel.Error;
            _console.WriteLine(message, outputLevel, null);
        }

    }
}