namespace Tharga.Quilt4Net.DataTransfer
{
    public class LoginResponse
    {
        private LoginResponse()
        {
        }

        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string userName { get; set; }
        //public string .issued { get; set; }
        //public string .expires { get; set; }
    }
}