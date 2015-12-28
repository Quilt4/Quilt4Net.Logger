using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.DataTransfer
{
    public class LoginResult : ILoginResult
    {
        internal LoginResult()
        {
        }

        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }
    }
}