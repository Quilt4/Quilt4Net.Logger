using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface IInvitation
    {
        Task CreateAsync(Guid projectKey, string user);
        Task<IEnumerable<InvitationResponse>> GetListAsync();
    }
}