using System;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal class UserHelper : IUserHelper
    {
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

        internal static string GetUserName()
        {
            throw new NotImplementedException();
            ////TODO: Does not work when using the '\' so now I tried with '-'. Perhaps '\\' will work.
            //return string.Format(@"{0}-{1}", Environment.UserDomainName, Environment.UserName);
        }

        private static string GetUserSid()
        {
            throw new NotImplementedException();
            //var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            //if (currentUser == null || currentUser.User == null || currentUser.User.AccountDomainSid == null) return "NULL";

            //return currentUser.User.AccountDomainSid.ToString();
        }
    }
}