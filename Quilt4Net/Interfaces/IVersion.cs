using System;
using System.Collections.Generic;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IVersion
    {
        string Name { get; }
        DateTime? BuildTime { get; }
        string SupportToolkit { get; }

        IEnumerable<IIssueType> IssueTypes { get; }

        //Up-links
        IApplication Application { get; }
        IProject Project { get; }

        //Shortcuts
        IEnumerable<ISession> Sessions { get; }
        IEnumerable<IUser> Users { get; }
        IEnumerable<IUserHandle> UserHandles { get; }
        IEnumerable<IMachine> Machines { get; }
        IEnumerable<IIssue> Issues { get; }
    }
}