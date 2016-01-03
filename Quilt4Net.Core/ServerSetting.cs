using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class ServerSetting : IServerSetting
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string _controller = "Client/Setting";

        internal ServerSetting(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task<IEnumerable<SettingResponse>> GetListAsync()
        {
            var response = await _webApiClient.ReadAsync<SettingResponse>(_controller);
            return response;
        }
    }
}