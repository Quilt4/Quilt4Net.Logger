namespace Quilt4Net
{
    public class LogAppData
    {
        public string Environment { get; set; }
        public string Application { get; set; }
        public string Version { get; set; }
        public string Machine { get; set; }
        public string SystemUser { get; set; }
        public LogDataItem[] Data { get; set; }
    }
}