using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class IssueType : IIssueType
    {
        private readonly Issue[] _issues;
        private readonly ISession[] _sessions;
        private readonly IUser[] _users;
        private readonly IUserHandle[] _userHandles;
        private readonly IMachine[] _machines;

        public IssueType(IProject project, IApplication application, IVersion version, IssueTypeResponse issueTypeResponse, IEnumerable<IssueResponse> issueResponses, IEnumerable<SessionResponse> sessionResponses)
        {
            Project = project;
            Version = version;
            Application = application;
            Message = issueTypeResponse.Message;
            StackTrace = new StackTrace(issueTypeResponse.StackTrace);
            Type = issueTypeResponse.Type;
            ResponseMessage = issueTypeResponse.ResponseMessage;
            Ticket = issueTypeResponse.Ticket;
            Level = new Issuelevel(issueTypeResponse.Level);
            //InnerIssue //TODO: Fix!
            _issues = issueResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == version.Name && x.IssueTypeMessage == Message).Select(x => new Issue(this, project.Sessions.Single(y => y.SessionKey == x.SessionKey), x)).ToArray();
            _sessions = project.Sessions.Where(x => _issues.Any(y => y.Session.SessionKey == x.SessionKey)).ToArray(); //Pick just the sessions where this issue type has appeared
            _users = _sessions.Select(x => x.User).Distinct().ToArray();
            _userHandles = _sessions.Select(x => x.UserHandle).Distinct().ToArray();
            _machines = _sessions.Select(x => x.Machine).Distinct().ToArray();
        }

        public string Message { get; }
        public IStackTrace StackTrace { get; }
        public string Type { get; }
        public string ResponseMessage { get; }
        public int Ticket { get; }
        public IIssuelevel Level { get; }
        public IIssueType InnerIssue { get { throw new NotImplementedException(); } }
        public IEnumerable<IIssue> Issues => _issues;
        public IEnumerable<ISession> Sessions => _sessions;
        public IVersion Version { get; }
        public IApplication Application { get; }
        public IProject Project { get; }
        public IEnumerable<IUser> Users => _users;
        public IEnumerable<IUserHandle> UserHandles => _userHandles;
        public IEnumerable<IMachine> Machines => _machines;
    }
}