using System;
using System.Threading.Tasks;
using Quilt4Console.Commands;
using Quilt4Console.Commands.Project;
using Quilt4Console.Commands.Service;
using Quilt4Console.Commands.User;
using Quilt4Net;
using Tharga.Toolkit.Console;
using Tharga.Toolkit.Console.Command.Base;
using Tharga.Toolkit.Registry;

namespace Quilt4Console
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var console = new ClientConsole();
            var rootCommand = new ProgramRootCommand(console);

            var configuration = new Quilt4Net.Configuration();
            var client = new Quilt4NetClient(configuration);
            var sessionHandler = new SessionHandler(client);
            var issueHandler = new IssueHandler(sessionHandler);
            new EventConsole(rootCommand, configuration, client, sessionHandler, issueHandler);

            LoadSettings(client);

            rootCommand.RegisterCommand(new ServiceCommands(client));
            rootCommand.RegisterCommand(new UserCommands(client));
            rootCommand.RegisterCommand(new ProjectCommands(client));
            new CommandEngine(rootCommand).Run(args);
        }

        private static async Task LoadSettings(Quilt4NetClient client)
        {
            var registry = new Setting();
            var address = await registry.GetSettingAsync("Target.Location", ELocalLevel.CurrentUser, client.Configuration.Target.Location);
            client.Configuration.Target.Location = address;

            Console.WriteLine("Connecting to quilt4 server " + address, OutputLevel.Information);
        }
    }
}