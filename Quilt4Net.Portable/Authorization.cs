namespace Quilt4Net
{
    internal class Authorization
    {
        public Authorization(string tokenType, string accessToken)
        {
            TokenType = tokenType;
            AccessToken = accessToken;
        }

        public string TokenType { get; }
        public string AccessToken { get; }
    }
}