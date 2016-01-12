using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Singleton
{
    public class Issue : IssueHandler
    {
        private Issue()
            : base(Session.Instance)
        {
        }

        public static IIssueHandler Instance { get; } = new Issue();
    }
}