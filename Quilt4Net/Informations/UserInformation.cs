using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    internal class UserInformation : UserInformationBase
    {
        internal UserInformation(IHashHandler hashHandler)
            : base(hashHandler)
        {
        }

        protected override string GetUserName()
        {
            return $@"{Environment.UserDomainName}\{Environment.UserName}";
        }

        protected override string GetUserSid()
        {
            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (currentUser == null || currentUser.User == null || currentUser.User.AccountDomainSid == null) return "NULL";
            return currentUser.User.AccountDomainSid.ToString();
        }
    }
}