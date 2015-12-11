using System;
using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class UserHelper : Core.UserHelper
    {
        public override UserData GetUser()
        {
            return base.GetUser(GetUserName());
        }

        public override string GetUserName()
        {
            //TODO: Does not work when using the '\' so now I tried with '-'. Perhaps '\\' will work.
            return string.Format(@"{0}-{1}", Environment.UserDomainName, Environment.UserName);
        }

        protected override string GetUserSid()
        {
            var currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            if (currentUser == null || currentUser.User == null || currentUser.User.AccountDomainSid == null) return "NULL";

            return currentUser.User.AccountDomainSid.ToString();
        }
    }

    public class MachineHelper : Quilt4Net.Core.MachineHelper
    {
        public override MachineData GetMachineData()
        {
            return base.GetMachineData(GetMachineName());
        }

        internal override string GetMachineName()
        {
            return Environment.MachineName;
        }
    }

    public class Session : Core.Session
    {
        public Session(IWebApiClient webApiClient, IApplicationHelper applicationHelper, IMachineHelper machineHelper, IUserHelper userHelper)
            : base(webApiClient, applicationHelper, machineHelper, userHelper)
        {
        }

        public override async Task RegisterAsync(string projectApiKey, string environment)
        {
            await base.RegisterAsync(projectApiKey, environment, Assembly.GetEntryAssembly());
        }
    }
}