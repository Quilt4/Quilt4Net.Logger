using System.Collections.Generic;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IApplication
    {
        string Name { get; }

        IEnumerable<IVersion> Versions { get; }
        IArchive Archive { get; }

        //Up-links
        IProject Project { get; }

        //Shortcuts
        IEnumerable<ISession> Sessions { get; }
        IEnumerable<IUser> Users { get; }
        IEnumerable<IUserHandle> UserHandles { get; }
        IEnumerable<IMachine> Machines { get; }
        IEnumerable<IIssue> Issues { get; }
        IEnumerable<IIssueType> IssueTypes { get; }
    }
}