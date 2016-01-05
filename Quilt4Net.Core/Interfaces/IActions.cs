namespace Quilt4Net.Core.Interfaces
{
    public interface IActions
    {
        IService Service { get; }
        IUser User { get; }
        IProject Project { get; }
        IInvitation Invitation { get; }
        IApplication Application { get; }
        IVersion Version { get; }
        IServerSetting ServerSetting { get; }
    }
}