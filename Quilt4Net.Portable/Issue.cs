using System;
using System.Threading.Tasks;
using Quilt4Net.Interfaces;

namespace Quilt4Net
{
    public class Issue
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string _controller = "Client/Issue";

        internal Issue(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task RegisterAsync()
        {
            throw new NotImplementedException();
        }
    }
}