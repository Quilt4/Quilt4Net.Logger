using System.Collections.Generic;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IProject
    {
        string Name { get; }
        IProjectInfo Info { get; }

        IEnumerable<IApplication> Applications { get; }

        //Shortcuts
        IEnumerable<ISession> Sessions { get; }
        IEnumerable<IUser> Users { get; }
        IEnumerable<IUserHandle> UserHandles { get; }
        IEnumerable<IMachine> Machines { get; }
        IEnumerable<IIssue> Issues { get; }
        IEnumerable<IIssueType> IssueTypes { get; }
        IEnumerable<IVersion> Versions { get; }
        //IVersion[] ArchivedVersions { get; }
    }
}