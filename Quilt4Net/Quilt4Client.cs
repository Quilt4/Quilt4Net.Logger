using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Quilt4Net.Core.WebApi;
using Action = Quilt4Net.Core.Actions.Action;
using IQuilt4NetClient = Quilt4Net.Interfaces.IQuilt4NetClient;

namespace Quilt4Net
{
    public class Quilt4Client : IQuilt4NetClient
    {
        private static readonly object _syncRoot = new object();
        private static int _instanceCounter;
        private readonly Lazy<IActions> _action;

        private readonly Lazy<IInformation> _information;
        private readonly Lazy<IClient> _signalRClient;

        public Quilt4Client(IConfiguration configuration)
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
            Configuration = configuration;
            _information = new Lazy<IInformation>(() => new Information(new ApplicationInformation(Configuration, hashHandler), new MachineInformation(hashHandler), new UserInformation(hashHandler)));
            _signalRClient = new Lazy<IClient>(() => new SignalRClient(configuration));
            _action = new Lazy<IActions>(() => new Action(Client));

            OnInstanceCreatedEvent(new InstanceCreatedEventArgs(this, _instanceCounter));
        }

        public IConfiguration Configuration { get; }

        public IClient Client => _signalRClient.Value;
        public IActions Actions => _action.Value;
        public IInformation Information => _information.Value;

        public static event EventHandler<InstanceCreatedEventArgs> InstanceCreatedEvent;

        private static void OnInstanceCreatedEvent(InstanceCreatedEventArgs e)
        {
            InstanceCreatedEvent?.Invoke(null, e);
        }
    }
}