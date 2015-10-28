namespace Tharga.Quilt4Net.DataTransfer
{
    public class IssueTypeResponse
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string ApplicationName { get; set; }
        public string VersionName { get; set; }
        public string Type { get; set; }
        public string ResponseMessage { get; set; }
        public int Ticket { get; set; }
        public string Level { get; set; }
        public IssueTypeResponse Inner { get; set; }
    }
}