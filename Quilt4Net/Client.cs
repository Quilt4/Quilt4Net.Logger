using System;

namespace Tharga.Quilt4Net
{
    public class Client
    {
        private readonly Lazy<User> _user;

        public Client(IWebApiClient webApiClient)
        {
            _user = new Lazy<User>(() => new User(webApiClient));
        }

        public User User => _user.Value;
    }
}