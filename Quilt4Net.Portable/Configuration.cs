using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal class Configuration : IConfiguration
    {
        public bool UseBuildTime { get { return true; } }
    }
}