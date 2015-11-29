namespace Tharga.Quilt4Net
{
    internal class Authorization
    {
        private readonly string _tokenType;
        private readonly string _accessToken;

        public Authorization(string tokenType, string accessToken)
        {
            _tokenType = tokenType;
            _accessToken = accessToken;
        }

        public string TokenType
        {
            get { return _tokenType; }
        }

        public string AccessToken
        {
            get { return _accessToken; }
        }
    }
}