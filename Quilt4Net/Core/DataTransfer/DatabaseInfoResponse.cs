namespace Quilt4Net.Core.DataTransfer
{
    public class DatabaseInfoResponse
    {
        public string Database { get; set; }
        public int Version { get; set; }
        public bool CanConnect { get; set; }
    }
}