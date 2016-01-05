namespace Quilt4Net.Core.Interfaces
{
    public interface IInformation
    {
        IApplicationInformation Aplication { get; }
        IMachineInformation Machine { get; }
        IUserInformation User { get; }
    }
}