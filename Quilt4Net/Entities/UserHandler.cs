using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class UserHandler : IUserHandle
    {
        public UserHandler(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}