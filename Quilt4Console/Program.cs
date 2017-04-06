﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Console.Commands;
using Quilt4Console.Commands.Issue;
using Quilt4Console.Commands.Project;
using Quilt4Console.Commands.Queue;
using Quilt4Console.Commands.Service;
using Quilt4Console.Commands.Session;
using Quilt4Console.Commands.Setting;
using Quilt4Console.Commands.User;
using Quilt4Net;
using Quilt4Net.Core;
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

            //var configuration = new Configuration();
            //var client = new Quilt4Client(configuration);
            //var sessionHandler = new SessionHandler(client);
            //var issueHandler = new IssueHandler(sessionHandler);
            //new EventConsole(rootCommand, configuration, client, sessionHandler, issueHandler);

            //LoadSettings(client);

            //rootCommand.RegisterCommand(new ServiceCommands(client));
            //rootCommand.RegisterCommand(new UserCommands(client));
            //rootCommand.RegisterCommand(new ProjectCommands(client));
            //rootCommand.RegisterCommand(new SettingCommands(client));
            ////rootCommand.RegisterCommand(new SessionCommands(sessionHandler));
            //rootCommand.RegisterCommand(new IssueCommands(sessionHandler, issueHandler));

            var client = new Quilt4Client(new Configuration());
            var issueHandler = new IssueHandler(new SessionHandler(client));
            issueHandler.RegisterStart("Blah1", MessageIssueLevel.Information, null, new Dictionary<string, string> { { "A", "A1" }, { "B", "B1" } });

            rootCommand.RegisterCommand(new QueueCommands(client.Client));

            new CommandEngine(rootCommand).Run(args);
        }

        //private static async Task LoadSettings(Quilt4Client client)
        //{
        //    var registry = new Setting();
        //    var address = await registry.GetSettingAsync("Target.Location", ELocalLevel.CurrentUser, client.Configuration.Target.Location);
        //    client.Configuration.Target.Location = address;

        //    Console.WriteLine("Connecting to quilt4 server " + address, OutputLevel.Information);
        //}
    }
}