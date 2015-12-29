using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class Application : IApplication
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string _controller = "Client/Application";

        internal Application(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task<IEnumerable<ApplicationResponse>> GetListAsync(Guid projectKey)
        {
            var response = await _webApiClient.ExecuteQueryAsync<Guid, IEnumerable<ApplicationResponse>>(_controller, "QueryByProject", projectKey);
            return response;
        }
    }
}