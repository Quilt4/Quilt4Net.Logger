using System;
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
                //Create a client that can be used to connect to the server with
                var client = new Client(new WebApiClient(new Uri("http://localhost:5004/"), new TimeSpan(0, 0, 0, 30)));

                //Create a new user to work with
                await client.User.CreateAsync("xyz", "uuu", "daniel.bohlin@gmail.com");

                //Logon using the new user
                var loginResponse = await client.User.Login("xyz", "uuu");
                System.Console.WriteLine(loginResponse.SessionKey);

                await client.Project.CreateAsync("Some project");
                var projects = await client.Project.GetAllAsync();
                foreach (var project in projects)
                {
                    System.Console.WriteLine("Project: " + project.Name);
                    var proj = await client.Project.GetAsync(project.ProjectId);
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception.Message + " @" + exception.StackTrace);
            }

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }
}