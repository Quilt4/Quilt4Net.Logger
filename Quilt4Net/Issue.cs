using System;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class Issue : Core.Issue
    {
        public Issue(Lazy<ISession> session, IWebApiClient webApiClient, IConfiguration configuration)
            : base(session, webApiClient, configuration)
        {
        }

        public static IIssue Instance => Client.Instance.Issue;
    }
}