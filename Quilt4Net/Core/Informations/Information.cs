using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal class Information : IInformation
    {
        internal Information(IApplicationInformation application, IMachineInformation machine, IUserInformation user)
        {
            Application = application;
            Machine = machine;
            User = user;
        }

        public IApplicationInformation Application { get; }
        public IMachineInformation Machine { get; }
        public IUserInformation User { get; }
    }
}