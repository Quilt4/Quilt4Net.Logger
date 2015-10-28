using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Issuelevel : IIssuelevel
    {
        public Issuelevel(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}