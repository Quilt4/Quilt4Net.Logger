namespace Tharga.Quilt4Net.DataTransfer
{
    public class LoginData
    {
        private LoginData()
        {
        }

        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }
    }
}