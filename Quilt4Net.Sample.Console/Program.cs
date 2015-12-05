using System;
using System.Linq;
using System.Threading.Tasks;
using Tharga.Quilt4Net;

namespace Quilt4Net.Sample.Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => MainAsync(args));

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }

        static async void MainAsync(string[] args)
        {
            try
            {
                var port = 29660; // 59779; //5000; //5004
                var client = new Client(new WebApiClient(new Uri($"http://localhost:{port}/"), new TimeSpan(0, 0, 0, 30)));

                //Note: Create a new user
                //await client.User.CreateAsync("daniel.bohlin@gmail.com", "Qwerty!1");

                //Note: Logon with a user
                await client.User.LoginAsync("daniel.bohlin@gmail.com", "Qwerty!1");

                //Note: Create a new project
                var projectKey = await client.Project.CreateAsync("A" + DateTime.Now.Millisecond.ToString("0"));

                //Note: GetA list of projects
                var projects = await client.Project.GetAllAsync();
                foreach (var project in projects)
                {
                    System.Console.WriteLine("Project: " + project.Name + ", ProjectApiKey: " + project.ProjectApiKey);

                    //Note: Get a single project
                    var a = await client.Project.GetAsync(project.ProjectKey);

                    //Note: Update a project
                    await client.Project.UpdateAsync(a.ProjectKey, a.Name + "_1", a.DashboardColor);
                }

                //Note: Register a session
                await client.Session.RegisterAsync(projects.First().ProjectApiKey, "Dev1");

                //Note: Delete a project
                await client.Project.DeleteAsync(projectKey);

                //Note: Logout
                await client.User.LogoutAsync();
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception.Message + " @" + exception.StackTrace);
            }
        }
    }
}