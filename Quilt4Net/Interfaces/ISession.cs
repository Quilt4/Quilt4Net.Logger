using System;
using System.Collections.Generic;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface ISession
    {
        Guid SessionKey { get; }
        DateTime StartTime { get; }
        DateTime? EndTime { get; }
        IEnvironment Environment { get; }
        string CallerIpAddress { get; }
        TimeSpan Duration { get; }
        IUser User { get; }
        IUserHandle UserHandle { get; }
        IMachine Machine { get; }

        //Shortcut / Up-links
        IEnumerable<IIssue> Issues { get; }
        IEnumerable<IIssueType> IssueTypes { get; }
        IVersion Version { get; }
        IApplication Application { get; }
        IProject Project { get; }
    }
}