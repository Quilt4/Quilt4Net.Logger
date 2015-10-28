using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Color : IColor
    {
        public Color(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}