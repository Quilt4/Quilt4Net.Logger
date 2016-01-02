using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public class Invitation : IInvitation
    {
        private readonly IWebApiClient _webApiClient;
        private readonly string _controller = "Client/Invitation";

        internal Invitation(IWebApiClient webApiClient)
        {
            _webApiClient = webApiClient;
        }

        public async Task CreateAsync(Guid projectKey, string user)
        {
            await _webApiClient.ExecuteCommandAsync(_controller, "InviteCommand", new InviteRequest { ProjectKey = projectKey, User = user });
        }

        public async Task<IEnumerable<InvitationResponse>> GetListAsync()
        {
            var result = await _webApiClient.ExecuteQueryAsync<string, IEnumerable<InvitationResponse>>(_controller, "UserInvitationQuery", null);
            return result;
        }
    }
}