using System;
using Quilt4Net.Core.Interfaces;
using Quilt4Net.Core.Lookups;

namespace Quilt4Net.Lookups
{
    internal class UserLookup : UserLookupBase
    {
        internal UserLookup(IHashHandler hashHandler)
            : base(hashHandler)
        {
        }

        protected override string GetUserName()
        {
            //Note: Does not work when using the '\' so now I tried with '-'. Perhaps '\\' will work.
            return string.Format(@"{0}-{1}", Environment.UserDomainName, Environment.UserName);
        }

        protected override string GetUserSid()
        {
            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (currentUser == null || currentUser.User == null || currentUser.User.AccountDomainSid == null) return "NULL";

            return currentUser.User.AccountDomainSid.ToString();
        }
    }
}