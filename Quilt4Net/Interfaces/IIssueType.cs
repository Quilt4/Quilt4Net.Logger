using System.Collections.Generic;

namespace Tharga.Quilt4Net.Interfaces
{
    public interface IIssueType
    {
        string Message { get; }
        IStackTrace StackTrace { get; }
        string Type { get; }
        string ResponseMessage { get; }
        int Ticket { get; }
        IIssuelevel Level { get; }
        IIssueType InnerIssue { get; }

        IEnumerable<IIssue> Issues { get; }

        //Up-links
        IVersion Version { get; }
        IApplication Application { get; }
        IProject Project { get; }

        //Shortcuts
        IEnumerable<ISession> Sessions { get; }
        IEnumerable<IUser> Users { get; }
        IEnumerable<IUserHandle> UserHandles { get; }
        IEnumerable<IMachine> Machines { get; }
    }
}