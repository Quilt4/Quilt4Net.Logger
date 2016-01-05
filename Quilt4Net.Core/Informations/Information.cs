using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal class Information : IInformation
    {
        internal Information(IApplicationInformation aplication, IMachineInformation machine, IUserInformation user)
        {
            Aplication = aplication;
            Machine = machine;
            User = user;
        }

        public IApplicationInformation Aplication { get; }
        public IMachineInformation Machine { get; }
        public IUserInformation User { get; }
    }
}