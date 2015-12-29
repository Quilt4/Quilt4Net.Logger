namespace Quilt4Net.Core.Interfaces
{
    public interface IAction
    {
        IService Service { get; }
        IUser User { get; }
        IProject Project { get; }
        IApplication Application { get; }
        IVersion Version { get; }
    }
}