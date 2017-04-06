namespace Quilt4Net.Singleton
{
    public class Quilt4Client : Quilt4Net.Quilt4Client
    {
        private Quilt4Client()
            : base(Singleton.Configuration.Instance)
        {
        }

        public static Interfaces.IQuilt4NetClient Instance { get; } = new Quilt4Client();
    }
}