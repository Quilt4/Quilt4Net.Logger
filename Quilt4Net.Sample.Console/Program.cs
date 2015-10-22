using System;
using System.Management.Instrumentation;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace Quilt4Net.Sample.Console
{
    class Program
    {
        private static readonly TimeSpan _timeout = new TimeSpan(0, 0, 0, 30);

        static void Main(string[] args)
        {
            //TODO: Make first API call. Create a user.

            var jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<CreateUserRequest>(new CreateUserRequest { Username = "xyz" }, jsonFormatter);

            var client = GetHttpClient();
            var response = client.PostAsync(string.Format("api/{0}/{1}", "user", "Create"), content);
            if (!response.Wait(_timeout))
                throw new TimeoutException("The WebAPI call exceeded the allotted time.");
            if (!response.Result.IsSuccessStatusCode)
            {
                throw new InvalidOperationException();
                //throw ExpectedIssues.GetException(ExpectedIssues.ServiceCallError).AddData("StatusCode", (int)response.Result.StatusCode).AddData("StatusCodeName", response.Result.StatusCode).AddData("ReasonPhrase", response.Result.ReasonPhrase);   
            }

            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }

        private static HttpClient GetHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:60638/") };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = _timeout;
            return client;
        }
    }

    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}