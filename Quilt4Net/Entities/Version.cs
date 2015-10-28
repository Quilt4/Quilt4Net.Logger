using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Version : IVersion
    {
        private readonly IssueType[] _issueTypes;
        private readonly IssueResponse[] _issueResponses;
        private readonly ISession[] _sessions;
        private readonly IUser[] _users;
        private readonly IUserHandle[] _userHandles;
        private readonly IMachine[] _machines;

        public Version(IProject project, IApplication application, VersionResponse versionResponse, IEnumerable<IssueTypeResponse> issueTypeResponses, IEnumerable<IssueResponse> issueResponses, SessionResponse[] sessionResponses)
        {
            Project = project;
            Application = application;
            Name = versionResponse.Name;
            BuildTime = versionResponse.BuildTime;
            SupportToolkit = versionResponse.SupportToolkit;
            _issueResponses = issueResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == Name).ToArray();
            var versionSessionResponses = sessionResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == Name).ToArray();
            _issueTypes = issueTypeResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == Name).Select(x => new IssueType(project, application, this, x, _issueResponses, versionSessionResponses)).ToArray();
            _sessions = project.Sessions.Where(x => sessionResponses.Any(y => y.ApplicationName == application.Name)).ToArray();
            _users = _sessions.Select(x => x.User).Distinct().ToArray();
            _userHandles = _sessions.Select(x => x.UserHandle).Distinct().ToArray();
            _machines = _sessions.Select(x => x.Machine).Distinct().ToArray();
        }

        public string Name { get; }
        public DateTime? BuildTime { get; }
        public string SupportToolkit { get; }
        public IApplication Application { get; }
        public IEnumerable<IIssueType> IssueTypes => _issueTypes;
        public IProject Project { get; }
        public IEnumerable<ISession> Sessions => _sessions;
        public IEnumerable<IUser> Users => _users;
        public IEnumerable<IUserHandle> UserHandles => _userHandles;
        public IEnumerable<IMachine> Machines => _machines;
        public IEnumerable<IIssue> Issues { get { return _issueTypes.SelectMany(x => x.Issues); } }
    }
}