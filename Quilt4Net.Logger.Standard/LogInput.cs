namespace Quilt4Net
{
    public class LogInput
    {
        public string CategoryName { get; set; }
        public int LogLevel { get; set; }
        public string Message { get; set; }
        public LogAppData AppData { get; set; }
        public LogDataItem[] Data { get; set; }
        public long? TimeInTicks { get; set; }
    }
}