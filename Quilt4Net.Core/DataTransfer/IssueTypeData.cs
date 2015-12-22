namespace Quilt4Net.Core.DataTransfer
{
    public class IssueTypeData
    {
        internal IssueTypeData()
        {
        }

        public string Message { get; internal set; }
        public string StackTrace { get; internal set; }
        public IssueLevel IssueLevel { get; internal set; }
        public string Type { get; internal set; }
        public IssueTypeData Inner { get; internal set; }
    }
}