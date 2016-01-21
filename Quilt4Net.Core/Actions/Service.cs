using System;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Actions
{
    public class Service : IService
    {
        private readonly IWebApiClient _webApiClient;

        internal Service(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
            Log = new Log(webApiClient);
        }

        public ILog Log { get; }

        public async Task<ServiceInfoResponse> GetServiceInfo()
        {
            return await _webApiClient.ExecuteQueryAsync<string, ServiceInfoResponse>("Service");
        }
    }
}