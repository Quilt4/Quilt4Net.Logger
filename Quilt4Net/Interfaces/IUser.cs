namespace Tharga.Quilt4Net.Interfaces
{
    public interface IUser
    {
        string UserKey { get; }        
        string Name { get; }

        //Shortcuts
        ISession[] Sessions { get; }
        IMachine[] Users { get; }
        IIssue[] Issues { get; }
        IIssueType[] IssueTypes { get; }
        IVersion[] Versions { get; }
        IApplication[] Applications { get; }
    }
}