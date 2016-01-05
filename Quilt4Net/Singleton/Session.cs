using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Singleton
{
    public class Session : SessionHandler
    {
        private Session()
            : base(Quilt4NetClient.Instance)
        {
        }

        public static ISessionHandler Instance { get; } = new Session();
    }
}