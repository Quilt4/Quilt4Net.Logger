using Quilt4Net.Core.Handlers;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class SessionHandler : SessionHandlerBase
    {
        public SessionHandler(IQuilt4NetClient client)
            : base(client.WebApiClient, client.Configuration, client.Lookup)
        {
        }

        internal SessionHandler(IWebApiClient webApiClient, IConfiguration configuration, IApplicationLookup applicationLookup, IMachineLookup machineLookup, IUserLookup userLookup)
            : base(webApiClient, configuration, applicationLookup, machineLookup, userLookup)
        {
        }
        
        //public static ISessionHandler Instance => Quilt4NetClient.Instance.SessionHandler;
    }
}