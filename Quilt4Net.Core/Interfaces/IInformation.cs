namespace Quilt4Net.Core.Interfaces
{
    public interface IInformation
    {
        IApplicationInformation Application { get; }
        IMachineInformation Machine { get; }
        IUserInformation User { get; }
    }
}