using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Action = Quilt4Net.Core.Actions.Action;

namespace Quilt4Net
{
    public class Quilt4NetClient : Interfaces.IQuilt4NetClient
    {
        private static readonly object _syncRoot = new object();
        private static int _instanceCounter;

        public static event EventHandler<InstanceCreatedEventArgs> InstanceCreatedEvent;

        private readonly IConfiguration _configuration;
        private readonly Lazy<IWebApiClient> _webApiClient;
        private readonly Lazy<IActions> _action;
        private readonly Lazy<IInformation> _information;

        public Quilt4NetClient(IConfiguration configuration)
        {
            lock (_syncRoot)
            {
                if (_instanceCounter != 0)
                {
                    if (!configuration.AllowMultipleInstances)
                    {
                        throw new InvalidOperationException("Multiple instances is not allowed. Set configuration setting AllowMultipleInstances to true if you want to use multiple instances of this object.");
                    }
                }
                _instanceCounter++;
            }

            var hashHandler = new HashHandler();
            _configuration = configuration;            
            _information = new Lazy<IInformation>(() => new Information(new ApplicationInformation(_configuration, hashHandler), new MachineInformation(hashHandler), new UserInformation(hashHandler)));
            _webApiClient = new Lazy<IWebApiClient>(() => new WebApiClient(_configuration));
            _action = new Lazy<IActions>(() => new Action(WebApiClient));

            OnInstanceCreatedEvent(new InstanceCreatedEventArgs(this, _instanceCounter));
        }

        public IConfiguration Configuration => _configuration;
        public IWebApiClient WebApiClient => _webApiClient.Value;
        public IActions Actions => _action.Value;
        public IInformation Information => _information.Value;

        private static void OnInstanceCreatedEvent(InstanceCreatedEventArgs e)
        {
            InstanceCreatedEvent?.Invoke(null, e);
        }
    }
}