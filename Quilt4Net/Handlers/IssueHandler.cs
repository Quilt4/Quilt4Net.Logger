using System;
using Quilt4Net.Core.Handlers;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Handlers
{
    public class IssueHandler : IssueHandlerBase
    {
        public IssueHandler(IQuilt4NetClient client)
            : base(new Lazy<ISessionHandler>(() => client.SessionHandler), client.WebApiClient,client.ConfigurationHandler)
        {
        }

        internal IssueHandler(Lazy<ISessionHandler> session, IWebApiClient webApiClient, IConfigurationHandler configurationHandler)
            : base(session, webApiClient, configurationHandler)
        {
        }

        //public static IIssueHandler Instance => Quilt4NetClient.Instance.IssueHandler;
    }
}