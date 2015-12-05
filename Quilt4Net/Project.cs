using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net
{
    public class Project
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string _controller = "Client/Project";

        internal Project(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task<IEnumerable<ProjectData>> GetAllAsync()
        {
            var result = await _webApiClient.ReadAsync<ProjectData>(_controller);
            return result;
        }

        public async Task<ProjectData> GetAsync(Guid projectKey)
        {
            var response = await _webApiClient.ReadAsync<ProjectData>(_controller, projectKey.ToString());
            return response;
        }

        public async Task<Guid> CreateAsync(string projectName, string dashboardColor = null)
        {
            var projectKey = Guid.NewGuid();
            await _webApiClient.CreateAsync(_controller, new ProjectData { ProjectKey = projectKey, Name = projectName, DashboardColor = dashboardColor });
            return projectKey;
        }

        public async Task UpdateAsync(Guid projectKey, string projectName, string dashboardColor)
        {
            await _webApiClient.UpdateAsync(_controller, projectKey.ToString(), new ProjectData { ProjectKey = projectKey, Name = projectName, DashboardColor = dashboardColor });
        }

        public async Task DeleteAsync(Guid projectKey)
        {
            await _webApiClient.DeleteAsync(_controller, projectKey.ToString());
        }
    }
}