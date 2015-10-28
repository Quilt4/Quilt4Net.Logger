using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Session : ISession
    {
        public Session(SessionResponse sessionResponse, IProject project, IUser user, IUserHandle userHandle, IMachine machine)
        {
            Project = project;
            User = user;
            UserHandle = userHandle ?? new UserHandler(string.Empty);
            Machine = machine;
            SessionKey = sessionResponse.SessionKey;
            StartTime = sessionResponse.StartTime;
            EndTime = sessionResponse.EndTime;
            Environment = new Environment(sessionResponse.EnvironmentName, sessionResponse.EnvironmentColor);
            CallerIpAddress = sessionResponse.CallerIpAddress;
        }

        public Guid SessionKey { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }
        public IEnvironment Environment { get; }
        public string CallerIpAddress { get; }
        public TimeSpan Duration => (EndTime ?? DateTime.UtcNow) - StartTime;
        public IUser User { get; }
        public IUserHandle UserHandle { get; }
        public IMachine Machine { get; }
        public IEnumerable<IIssue> Issues { get { return Project.Issues.Where(x => x.Session.SessionKey == SessionKey); } }
        public IEnumerable<IIssueType> IssueTypes { get { return Project.IssueTypes.Where(x => x.Sessions.Any(y => y.SessionKey == SessionKey )); } }
        public IVersion Version { get { return Project.Versions.Single(x => x.Sessions.Any(y => y.SessionKey == SessionKey)); } }
        public IApplication Application { get { return Project.Applications.Single(x => x.Sessions.Any(y => y.SessionKey == SessionKey)); } }
        public IProject Project { get; }
    }
}