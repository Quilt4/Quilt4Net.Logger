using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal abstract class UserInformationBase : IUserInformation
    {
        private readonly IHashHandler _hashHandler;

        protected abstract string GetUserName();
        protected abstract string GetUserSid();

        protected UserInformationBase(IHashHandler hashHandler)
        {
            _hashHandler = hashHandler;
        }

        public UserData GetDataUser()
        {
            var userName = GetUserName();
            var fingerprint = $"UI1:{_hashHandler.ToMd5Hash($"{GetUserSid()}{userName}")}";

            var user = new UserData
            {
                Fingerprint = fingerprint,
                UserName = userName,
            };
            return user;
        }
    }
}