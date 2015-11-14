namespace Tharga.Quilt4Net.DataTransfer
{
    public class LoginResponse
    {
        private LoginResponse()
        {
        }

        public string PublicSessionKey { get; set; }
        public string PrivateSessionKey { get; set; }
    }
}