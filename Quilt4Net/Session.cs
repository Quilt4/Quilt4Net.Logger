using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class Session : Core.Session
    {
        private static Client _client;

        public Session(IWebApiClient webApiClient, IConfiguration configuration, IApplicationHelper applicationHelper, IMachineHelper machineHelper, IUserHelper userHelper)
            : base(webApiClient, configuration, applicationHelper, machineHelper, userHelper)
        {
        }

        internal static Client Client
        {
            get
            {
                //TODO: Use locking here

                if (_client == null)
                {
                    _client = new Client();
                }

                return _client;
            }
        }

        public static ISession Instance
        {
            get
            {
                return Client.Session;
            }
        }
    }
}