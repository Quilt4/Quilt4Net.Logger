using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Informations
{
    public class Information : IInformation
    {
        public Information(IApplicationInformation aplicationInformation, IMachineInformation machineInformation, IUserInformation userInformation)
        {
            AplicationInformation = aplicationInformation;
            MachineInformation = machineInformation;
            UserInformation = userInformation;
        }

        public IApplicationInformation AplicationInformation { get; }
        public IMachineInformation MachineInformation { get; }
        public IUserInformation UserInformation { get; }
    }
}