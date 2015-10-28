using System.Collections.Generic;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IMachine
    {
        string MachineKey { get; }
        string Name { get; }
        IDictionary<string, string> Data { get; }

        //Shortcuts
        ISession[] Sessions { get; }
        IUser[] Users { get; }
        IUserHandle[] UserHandles { get; }
        IIssue[] Issues { get; }
        IIssueType[] IssueTypes { get; }
        IVersion[] Versions { get; }
        IApplication[] Applications { get; }
        IProject[] Projects { get; }
    }
}