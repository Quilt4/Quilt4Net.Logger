using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Domain
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

        public async Task<IEnumerable<IProjectInfo>> GetAllAsync()
        {
            var result = await _webApiClient.ExecuteGetList<IProjectInfo>("project");
            return result;
        }

        public async Task<IProject> GetAsync(Guid projectId)
        {
            //Step1. Call service to get a response
            var result = await _webApiClient.ExecuteGet<Guid, ProjectResponse>("project", projectId);

            //Step2. Build the domain model
            var project = new Entities.Project(result);

            return project;
        }
    }
}