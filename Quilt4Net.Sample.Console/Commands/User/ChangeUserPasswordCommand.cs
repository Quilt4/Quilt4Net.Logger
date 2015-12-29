using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Command.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class ChangeUserPasswordCommand : ActionCommandBase
    {
        private readonly IClient _client;

        public ChangeUserPasswordCommand(IClient client)
            : base("password", "Change user password.")
        {
            _client = client;
        }

        public override async Task<bool> InvokeAsync(string paramList)
        {
            var index = 0;
            var oldPassword = QueryParam<string>("Old Password", GetParam(paramList, index++));
            var newPassword = QueryParam<string>("New Password", GetParam(paramList, index++));
            var confirmPassword = QueryParam<string>("Confirm Password", GetParam(paramList, index++));

            await _client.Action.User.ChangePassword(oldPassword, newPassword, confirmPassword);

            return true;
        }
    }
}