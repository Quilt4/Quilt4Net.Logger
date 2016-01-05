namespace Quilt4Net.Core.Interfaces
{
    public interface IInformation
    {
        IApplicationInformation AplicationInformation { get; }
        IMachineInformation MachineInformation { get; }
        IUserInformation UserInformation { get; }
    }
}