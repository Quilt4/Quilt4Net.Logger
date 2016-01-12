using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;
using Action = Quilt4Net.Core.Actions.Action;

namespace Quilt4Net
{
    public class Quilt4NetClient : IQuilt4NetClient
    {
        private readonly IConfiguration _configuration;
        private readonly Lazy<IWebApiClient> _webApiClient;
        private readonly Lazy<IActions> _action;
        private readonly Lazy<IInformation> _information;

        public Quilt4NetClient(IConfiguration configuration)
        {
            var hashHandler = new HashHandler();
            _configuration = configuration;
            _information = new Lazy<IInformation>(() => new Information(new ApplicationInformation(_configuration, hashHandler), new MachineInformation(hashHandler), new UserInformation(hashHandler)));
            _webApiClient = new Lazy<IWebApiClient>(() => new WebApiClient(_configuration));
            _action = new Lazy<IActions>(() => new Action(WebApiClient));
        }

        public IConfiguration Configuration => _configuration;
        public IWebApiClient WebApiClient => _webApiClient.Value;
        public IActions Actions => _action.Value;
        public IInformation Information => _information.Value;
    }
}