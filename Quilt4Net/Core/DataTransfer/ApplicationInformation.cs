namespace Quilt4Net.Core.Interfaces
{
    public class ApplicationNameVersion
    {
        internal ApplicationNameVersion(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; }
        public string Version { get; }
    }
}