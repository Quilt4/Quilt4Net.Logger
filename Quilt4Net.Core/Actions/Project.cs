using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Actions
{
    public class Project : IProject
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string _controller = "Client/Project";

        internal Project(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task<IEnumerable<ProjectResponse>> GetListAsync()
        {
            var result = await _webApiClient.ReadAsync<ProjectResponse>(_controller);
            return result;
        }

        public async Task<ProjectResponse> GetAsync(Guid projectKey)
        {
            var response = await _webApiClient.ReadAsync<ProjectResponse>(_controller, projectKey.ToString());
            return response;
        }

        public async Task<Guid> CreateAsync(string projectName, string dashboardColor = null)
        {
            var projectKey = Guid.NewGuid();
            await _webApiClient.CreateAsync(_controller, new ProjectRequest { ProjectKey = projectKey, Name = projectName, DashboardColor = dashboardColor });
            return projectKey;
        }

        public async Task UpdateAsync(Guid projectKey, string projectName, string dashboardColor)
        {
            await _webApiClient.UpdateAsync(_controller, projectKey.ToString(), new ProjectRequest { ProjectKey = projectKey, Name = projectName, DashboardColor = dashboardColor });
        }

        public async Task DeleteAsync(Guid projectKey)
        {
            await _webApiClient.DeleteAsync(_controller, projectKey.ToString());
        }

        public async Task<IEnumerable<MemberResponse>> GetMembersAsync(Guid projectKey)
        {
            var response = await _webApiClient.ExecuteQueryAsync<Guid, IEnumerable<MemberResponse>>(_controller, "ProjectMemberQuery", projectKey);
            return response;
        }
    }
}