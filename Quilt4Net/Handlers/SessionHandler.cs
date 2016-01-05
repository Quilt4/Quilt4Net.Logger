using Quilt4Net.Core.Handlers;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Handlers
{
    public class SessionHandler : SessionHandlerBase
    {
        public SessionHandler(IQuilt4NetClient client)
            : base(client.WebApiClient, client.ConfigurationHandler, client.Lookup)
        {
        }

        internal SessionHandler(IWebApiClient webApiClient, IConfigurationHandler configurationHandler, IApplicationLookup applicationLookup, IMachineLookup machineLookup, IUserLookup userLookup)
            : base(webApiClient, configurationHandler, applicationLookup, machineLookup, userLookup)
        {
        }
        
        //public static ISessionHandler Instance => Quilt4NetClient.Instance.SessionHandler;
    }
}