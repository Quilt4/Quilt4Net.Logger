using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Actions
{
    public class Log : ILog
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string _controller = "Client/Service/Log";

        internal Log(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task<IEnumerable<ServiceLogResponse>> GetListAsync()
        {
            var response = await _webApiClient.ReadAsync<ServiceLogResponse>(_controller);
            return response;
        }
    }
}