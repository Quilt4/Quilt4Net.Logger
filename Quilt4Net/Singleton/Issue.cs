namespace Quilt4Net.Singleton
{
    public class Issue : IssueHandler
    {
        private Issue()
            : base(Session.Instance)
        {
        }

        public static Interfaces.IIssueHandler Instance { get; } = new Issue();
    }
}