using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public abstract class UserHelper : IUserHelper
    {
        protected abstract string GetUserName();
        protected abstract string GetUserSid();

        public UserData GetUser()
        {
            var userName = GetUserName();
            var fingerprint = $"UI1:{$"{GetUserSid()}{userName}".ToMd5Hash()}";

            var user = new UserData
            {
                Fingerprint = fingerprint,
                UserName = userName,
            };
            return user;
        }
    }
}