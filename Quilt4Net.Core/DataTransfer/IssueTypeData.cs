namespace Quilt4Net.Core.DataTransfer
{
    public class IssueTypeData
    {
        internal IssueTypeData()
        {
        }

        public string Message { get; set; }
        public string StackTrace { get; set; }
        public IssueLevel IssueLevel { get; set; }
        public string Type { get; set; }
        public IssueTypeData Inner { get; set; }
    }
}