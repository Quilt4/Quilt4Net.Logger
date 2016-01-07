using System;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class IssueHandler : IssueHandlerBase
    {
        public IssueHandler(IQuilt4NetClient client)
            : base(new Lazy<ISessionHandler>(() => client.Session), client.WebApiClient,client.Configuration)
        {
            //TODO: The client must somehow get a reference to this object, or the IOC thing will not work.
            //(Or remove the session object from the client)
        }

        internal IssueHandler(Lazy<ISessionHandler> session, IWebApiClient webApiClient, IConfiguration configuration)
            : base(session, webApiClient, configuration)
        {
        }
    }
}