using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class Version : IVersion
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string _controller = "Client/Version";

        internal Version(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task<IEnumerable<VersionResponse>> GetListAsync(Guid applicationKey)
        {
            var response = await _webApiClient.ExecuteQueryAsync<Guid, IEnumerable<VersionResponse>>(_controller, "QueryByApplication", applicationKey);
            return response;
        }
    }
}