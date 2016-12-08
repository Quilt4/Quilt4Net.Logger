using System;
using Quilt4Console.Commands;
using Quilt4Console.Commands.Project;
using Quilt4Console.Commands.Service;
using Quilt4Console.Commands.User;
using Quilt4Net;
using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Console
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var console = new ClientConsole();
            var rootCommand = new ProgramRootCommand(console);

            var configuration = new Configuration();
            var client = new Quilt4NetClient(configuration);
            //var sessionHandler = new SessionHandler(client);
            //var issueHandler = new IssueHandler(sessionHandler);

            rootCommand.RegisterCommand(new ServiceCommands(client));
            rootCommand.RegisterCommand(new UserCommands(client));
            rootCommand.RegisterCommand(new ProjectCommands(client));
            new CommandEngine(rootCommand).Run(args);
        }
    }
}