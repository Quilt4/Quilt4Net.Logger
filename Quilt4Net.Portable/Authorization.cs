namespace Quilt4Net.Core
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