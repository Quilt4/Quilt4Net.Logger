using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Environment : IEnvironment
    {
        public Environment(string name, string color)
        {
            Name = name;
            Color = new Color(color);
        }

        public string Name { get; }
        public IColor Color { get; }
    }
}