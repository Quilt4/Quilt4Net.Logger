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
        }

        static async void MainAsync(string[] args)
        {
            var client = new Client(new WebApiClient(new Uri("http://localhost:60638/"), new TimeSpan(0, 0, 0, 30)));
            await client.User.CreateAsync("xyz", "uuu", "daniel.bohlin@gmail.com");
            var response = await client.User.Login("xyz", "INVALID");

            System.Console.WriteLine(response.SessionKey);

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }
}