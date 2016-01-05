using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Singleton
{
    public class Configuration : Quilt4Net.Configuration
    {
        private Configuration()
        {
        }

        public static IConfiguration Instance { get; } = new Configuration();
    }
}