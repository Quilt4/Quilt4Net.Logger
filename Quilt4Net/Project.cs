using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net
{
    public class Project
    {
        private readonly IWebApiClient _webApiClient;

        internal Project(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task CreateAsync(string projectName)
        {
            await _webApiClient.ExecuteCreateCommandAsync("project", new ProjectRequest { Name = projectName });
        }

        public async Task<IEnumerable<ProjectResponse>> GetAllAsync()
        {
            var result = await _webApiClient.ExecuteGetList<ProjectResponse>("project");
            return result;
        }
    }
}