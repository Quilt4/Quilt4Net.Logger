using Quilt4Net.Core.Handlers;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class SessionHandler : SessionHandlerBase
    {
        public SessionHandler(IQuilt4NetClient client)
            : base(client.WebApiClient, client.Configuration, client.Information)
        {
        }

        internal SessionHandler(IWebApiClient webApiClient, IConfiguration configuration, IApplicationInformation applicationInformation, IMachineInformation machineInformation, IUserInformation userInformation)
            : base(webApiClient, configuration, applicationInformation, machineInformation, userInformation)
        {
        }
        
        //public static ISessionHandler Instance => Quilt4NetClient.Instance.SessionHandler;
    }
}