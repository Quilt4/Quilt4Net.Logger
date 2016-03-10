namespace Quilt4Net.Singleton
{
    public class Session : SessionHandler
    {
        private Session()
            : base(Quilt4NetClient.Instance)
        {
        }

        public static Interfaces.ISessionHandler Instance { get; } = new Session();
    }
}