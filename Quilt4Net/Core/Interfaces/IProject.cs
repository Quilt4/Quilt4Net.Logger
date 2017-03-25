using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface IProject
    {
        Task<Guid> CreateAsync(string projectName, string dashboardColor = null);
        Task<IEnumerable<ProjectResponse>> GetListAsync();
        Task<ProjectResponse> GetAsync(Guid projectKey);
        Task UpdateAsync(Guid projectKey, string projectName, string dashboardColor);
        Task DeleteAsync(Guid projectKey);
        Task<IEnumerable<MemberResponse>> GetMembersAsync(Guid projectKey);
    }
}