using System;
using Quilt4Net.Core.Handlers;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class IssueHandler : IssueHandlerBase
    {
        public IssueHandler(IQuilt4NetClient client)
            : base(new Lazy<ISessionHandler>(() => client.Session), client.WebApiClient,client.Configuration)
        {
        }

        internal IssueHandler(Lazy<ISessionHandler> session, IWebApiClient webApiClient, IConfiguration configuration)
            : base(session, webApiClient, configuration)
        {
        }

        //public static IIssueHandler Instance => Quilt4NetClient.Instance.IssueHandler;
    }
}