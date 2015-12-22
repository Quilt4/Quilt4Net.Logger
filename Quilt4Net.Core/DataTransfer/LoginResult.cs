namespace Quilt4Net.Core.DataTransfer
{
    public class LoginResult
    {
        private LoginResult()
        {
        }

        public string access_token { get; internal set; }
        public string token_type { get; internal set; }
        public string expires_in { get; internal set; }
        public string userName { get; internal set; }
    }
}