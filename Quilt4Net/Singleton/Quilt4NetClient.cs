using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Singleton
{
    public class Quilt4NetClient : Quilt4Net.Quilt4NetClient
    {
        private Quilt4NetClient()
            : base(Singleton.Configuration.Instance)
        {
        }

        public static IQuilt4NetClient Instance { get; } = new Quilt4NetClient();
    }
}