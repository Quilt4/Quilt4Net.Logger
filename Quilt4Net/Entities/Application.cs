using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Application : IApplication
    {
        private readonly Version[] _versions;
        private readonly IssueTypeResponse[] _issueTypeResponses;
        private readonly IssueResponse[] _issueResponses;
        private readonly ISession[] _sessions;
        private readonly IUser[] _users;
        private readonly IUserHandle[] _userHandles;
        private readonly IMachine[] _machines;

        internal Application(IProject project, ApplicationResponse applicationResponse, IEnumerable<VersionResponse> versionResponses, IEnumerable<IssueTypeResponse> issueTypeResponses, IEnumerable<IssueResponse> issueResponses, SessionResponse[] sessionResponses)
        {
            Project = project;
            Name = applicationResponse.Name;
            _issueTypeResponses = issueTypeResponses.Where(x => x.ApplicationName == Name).ToArray();
            _issueResponses = issueResponses.Where(x => x.ApplicationName == Name).ToArray();
            var applicationSessionResponses = sessionResponses.Where(x => x.ApplicationName == Name).ToArray();
            _versions = versionResponses.Where(x => x.ApplicationName == Name).Select(x => new Version(project, this, x, _issueTypeResponses, _issueResponses, applicationSessionResponses)).ToArray();
            _sessions = project.Sessions.Where(x => sessionResponses.Any(y => y.ApplicationName == Name)).ToArray();
            _users = _sessions.Select(x => x.User).Distinct().ToArray();
            _userHandles = _sessions.Select(x => x.UserHandle).Distinct().ToArray();
            _machines = _sessions.Select(x => x.Machine).Distinct().ToArray();
        }

        public string Name { get; }
        public IProject Project { get; }
        public IEnumerable<IVersion> Versions => _versions;
        public IEnumerable<ISession> Sessions => _sessions;
        public IEnumerable<IUser> Users => _users;
        public IEnumerable<IUserHandle> UserHandles => _userHandles;
        public IEnumerable<IMachine> Machines => _machines;
        public IEnumerable<IIssue> Issues { get { return _versions.SelectMany(x => x.Issues); } }
        public IEnumerable<IIssueType> IssueTypes { get { return _versions.SelectMany(x => x.IssueTypes); } }

        //TODO: Populate this
        public IArchive Archive { get { throw new NotImplementedException(); } }
    }
}