using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class BasicDataHandler
    {
        private readonly IWebApiClient _webApiClient;

        public BasicDataHandler(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task<string> Post(string controller, string jsonData)
        {
            return await _webApiClient.PostAsync(controller, jsonData);
        }
    }
}