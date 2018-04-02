using Quilt4Net.Core.Interfaces;
using Tharga.Toolkit.Console.Commands.Base;

namespace Quilt4Net.Sample.Console.Commands.User
{
    internal class ChangeUserPasswordCommand : ActionCommandBase
    {
        private readonly IQuilt4NetClient _client;

        public ChangeUserPasswordCommand(IQuilt4NetClient client)
            : base("password", "Change user password.")
        {
            _client = client;
        }

        public override void Invoke(string[] param)
        {
            var oldPassword = QueryParam<string>("Old Password", param);
            var newPassword = QueryParam<string>("New Password", param);
            var confirmPassword = QueryParam<string>("Confirm Password", param);

            _client.Actions.User.ChangePassword(oldPassword, newPassword, confirmPassword);
        }
    }
}