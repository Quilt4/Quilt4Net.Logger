using System;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public abstract class UserHelper : IUserHelper
    {
        public abstract UserData GetUser();

        public UserData GetUser(string userName)
        {
            var fingerprint = $"UI1:{$"{GetUserSid()}{userName}".ToMd5Hash()}";

            var user = new UserData
            {
                Fingerprint = fingerprint,
                UserName = userName,
            };
            return user;
        }

        public abstract string GetUserName();
        //{
        //    throw new NotImplementedException();
        //    ////TODO: Does not work when using the '\' so now I tried with '-'. Perhaps '\\' will work.
        //    //return string.Format(@"{0}-{1}", Environment.UserDomainName, Environment.UserName);
        //}

        protected virtual string GetUserSid()
        {
            return string.Empty;
        }
    }
}