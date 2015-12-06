using Quilt4Net.Interfaces;

namespace Quilt4Net
{
    internal class Configuration : IConfiguration
    {
        public bool UseBuildTime { get { return true; } }
    }
}