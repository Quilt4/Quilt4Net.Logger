using System;
using Quilt4Net.Sample.Console.Commands.Project;
using Quilt4Net.Sample.Console.Commands.Session;
using Quilt4Net.Sample.Console.Commands.User;
using Tharga.Quilt4Net;
using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            var port = 29660;
            var address = new Uri($"http://localhost:{port}/");
            var client = new Client(new WebApiClient(address, new TimeSpan(0, 0, 0, 30)));

            var console = new ClientConsole();
            console.WriteLine("Using serer " + address, OutputLevel.Information, null);

            var rootCommand = new RootCommand(console);
            rootCommand.RegisterCommand(new UserCommands(client));
            rootCommand.RegisterCommand(new ProjectCommands(client));
            rootCommand.RegisterCommand(new SessionCommands(client));
            new CommandEngine(rootCommand).Run(args);
        }
    }
}