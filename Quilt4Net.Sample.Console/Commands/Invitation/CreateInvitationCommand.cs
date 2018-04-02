using System.Linq;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.Invitation
{
    internal class CreateInvitationCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public CreateInvitationCommand(IQuilt4NetClient client)
            : base("Create", "Create a new invitation.")
        {
            _client = client;
        }

        public override void Invoke(string[] param)
        {
            var projectKey = QueryParam("Project", param, _client.Actions.Project.GetListAsync().Result.ToDictionary(x => x.ProjectKey, x => x.Name));
            var user = QueryParam<string>("UserName/EMail", param);

            _client.Actions.Invitation.CreateAsync(projectKey, user);
        }
    }
}