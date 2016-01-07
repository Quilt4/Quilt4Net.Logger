using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class SessionHandler : SessionHandlerBase
    {
        public SessionHandler(IQuilt4NetClient client)
            : base(client.WebApiClient, client.Configuration, client.Information)
        {
            //TODO: The client must somehow get a reference to this object, or the IOC thing will not work.
            //(Or remove the session object from the client)
        }

        internal SessionHandler(IWebApiClient webApiClient, IConfiguration configuration, IApplicationInformation applicationInformation, IMachineInformation machineInformation, IUserInformation userInformation)
            : base(webApiClient, configuration, applicationInformation, machineInformation, userInformation)
        {
        }
    }
}