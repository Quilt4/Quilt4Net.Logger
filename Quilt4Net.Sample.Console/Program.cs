using System;
using Quilt4Net.Sample.Console.Commands.Project;
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
            new CommandEngine(rootCommand).Run(args);
        }

        //static async void MainAsync(string[] args)
        //{
        //    try
        //    {
        //        var port = 29660; // 59779; //5000; //5004
        //        var client = new Client(new WebApiClient(new Uri($"http://localhost:{port}/"), new TimeSpan(0, 0, 0, 30)));

        //        //Note: Create a new user
        //        //await client.User.CreateAsync("daniel.bohlin@gmail.com", "Qwerty!1");

        //        //Note: Logon with a user
        //        await client.User.LoginAsync("daniel.bohlin@gmail.com", "Qwerty!1");

        //        //Note: Create a new project
        //        var projectKey = await client.Project.CreateAsync("A" + DateTime.Now.Millisecond.ToString("0"));

        //        //Note: GetA list of projects
        //        var projects = await client.Project.GetListAsync();
        //        foreach (var project in projects)
        //        {
        //            System.Console.WriteLine("Project: " + project.Name + ", ProjectApiKey: " + project.ProjectApiKey);

        //            //Note: Get a single project
        //            //var a = await client.Project.GetAsync(project.ProjectKey);

        //            //Note: Update a project
        //            //await client.Project.UpdateAsync(a.ProjectKey, a.Name + "_1", a.DashboardColor);
        //        }

        //        //Note: Register a session
        //        await client.Session.RegisterAsync(projects.First().ProjectApiKey, "Dev1");

        //        //Note: Get a list of all sessions that the current user have access to
        //        var sessions = await client.Session.GetListAsync();
        //        foreach (var session in sessions)
        //        {
        //            System.Console.WriteLine("SessionKey: " + session.SessionKey);
        //        }

        //        //Note: Register an issue
        //        await client.Issue.RegisterAsync();

        //        //Note: Delete a project
        //        await client.Project.DeleteAsync(projectKey);

        //        //Note: Logout
        //        await client.User.LogoutAsync();
        //    }
        //    catch (Exception exception)
        //    {
        //        System.Console.WriteLine(exception.Message + " @" + exception.StackTrace);
        //    }
        //}
    }
}