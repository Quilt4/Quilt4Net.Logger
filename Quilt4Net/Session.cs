using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class Session : Core.Session
    {
        public Session(IWebApiClient webApiClient, IConfiguration configuration, IApplicationHelper applicationHelper, IMachineHelper machineHelper, IUserHelper userHelper)
            : base(webApiClient, configuration, applicationHelper, machineHelper, userHelper)
        {
        }
    }
}