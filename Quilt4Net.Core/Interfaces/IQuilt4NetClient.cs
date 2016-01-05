using System;

namespace Quilt4Net.Core.Interfaces
{
    public interface IQuilt4NetClient : IDisposable
    {
        IIssueHandler Issue { get; }
        ISessionHandler Session { get; }
        IActions Actions { get; }
        IInformation Information { get; }
        IWebApiClient WebApiClient { get; }
        IConfiguration Configuration { get; }
    }
}