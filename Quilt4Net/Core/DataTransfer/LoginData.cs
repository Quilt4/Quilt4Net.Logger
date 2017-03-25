namespace Quilt4Net.Core.DataTransfer
{
    internal class LoginData
    {
        internal LoginData()
        {
        }

        public string grant_type { get; internal set; }
        public string username { get; internal set; }
        public string password { get; internal set; }
    }
}