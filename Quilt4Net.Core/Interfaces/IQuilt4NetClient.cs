using System;

namespace Quilt4Net.Core.Interfaces
{
    public interface IQuilt4NetClient : IDisposable
    {
        IIssueHandler IssueHandler { get; }
        ISessionHandler SessionHandler { get; }
        IActions Actions { get; }
        ILookup Lookup { get; }
        IWebApiClient WebApiClient { get; }
        IConfigurationHandler ConfigurationHandler { get; }
    }
}