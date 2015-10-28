using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class UserHandler : IUserHandle
    {
        public UserHandler(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    internal class User : IUser
    {
        public User(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public ISession[] Sessions { get { throw new NotImplementedException(); } }
        public IMachine[] Users { get { throw new NotImplementedException(); } }
        public IIssue[] Issues { get { throw new NotImplementedException(); } }
        public IIssueType[] IssueTypes { get { throw new NotImplementedException(); } }
        public IVersion[] Versions { get { throw new NotImplementedException(); } }
        public IApplication[] Applications { get { throw new NotImplementedException(); } }

        //public IProject[] Projects { get { throw new NotImplementedException(); } }
    }

    internal class Color : IColor
    {
        public Color(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    internal class Environment : IEnvironment
    {
        public Environment(string name, string color)
        {
            Name = name;
            Color = new Color(color);
        }

        public string Name { get; }
        public IColor Color { get; }
    }

    internal class Session : ISession
    {
        public Session(SessionResponse sessionResponse, IProject project, IUser user, IUserHandle userHandle)
        {
            Project = project;
            User = user;
            UserHandle = userHandle ?? new UserHandler(string.Empty);
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
        public IMachine Machine { get { throw new NotImplementedException(); } }
        //public IIssue[] Issues { get { throw new NotImplementedException(); } }
        //public IIssueType[] IssueTypes { get { throw new NotImplementedException(); } }
        //public IVersion Version { get { return Project.Applications.SelectMany(x => x.Versions).Single(x => x.Name == _sessionResponse.VersionName); } }
        //public IApplication Application { get { return Project.Applications.Single(x => x.Name == _sessionResponse.ApplicationName); } }
        public IProject Project { get; }
    }

    internal class Issuelevel : IIssuelevel
    {
        public Issuelevel(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    internal class StackTrace : IStackTrace
    {
        public StackTrace(string stackTrace)
        {            
        }
    }

    internal class IssueType : IIssueType
    {
        private readonly Issue[] _issues;
        private readonly ISession[] _sessions;
        private readonly IUser[] _users;
        private readonly IUserHandle[] _userHandles;

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

        //TODO: Populate this
        public IMachine[] Machines { get { throw new NotImplementedException(); } }
    }

    internal class Version : IVersion
    {
        private readonly IssueType[] _issueTypes;
        private readonly IssueResponse[] _issueResponses;
        private readonly ISession[] _sessions;
        private readonly IUser[] _users;
        private readonly IUserHandle[] _userHandles;

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

        //TODO: Populate this
        public IMachine[] Machines { get { throw new NotImplementedException(); } }
        public IIssue[] Issues { get { throw new NotImplementedException(); } }
    }

    internal class Application : IApplication
    {
        private readonly Version[] _versions;
        private readonly IssueTypeResponse[] _issueTypeResponses;
        private readonly IssueResponse[] _issueResponses;
        private readonly ISession[] _sessions;
        private readonly IUser[] _users;
        private readonly IUserHandle[] _userHandles;

        internal Application(IProject project, ApplicationResponse applicationResponse, IEnumerable<VersionResponse> versionResponses, IEnumerable<IssueTypeResponse> issueTypeResponses, IEnumerable<IssueResponse> issueResponses, SessionResponse[] sessionResponses)
        {
            Name = applicationResponse.Name;
            _issueTypeResponses = issueTypeResponses.Where(x => x.ApplicationName == Name).ToArray();
            _issueResponses = issueResponses.Where(x => x.ApplicationName == Name).ToArray();
            var applicationSessionResponses = sessionResponses.Where(x => x.ApplicationName == Name).ToArray();
            _versions = versionResponses.Where(x => x.ApplicationName == Name).Select(x => new Version(project, this, x, _issueTypeResponses, _issueResponses, applicationSessionResponses)).ToArray();
            _sessions = project.Sessions.Where(x => sessionResponses.Any(y => y.ApplicationName == Name)).ToArray();
            _users = _sessions.Select(x => x.User).Distinct().ToArray();
            _userHandles = _sessions.Select(x => x.UserHandle).Distinct().ToArray();
        }

        public string Name { get; }
        public IEnumerable<IVersion> Versions => _versions;
        public IEnumerable<ISession> Sessions => _sessions;
        public IEnumerable<IUser> Users => _users;
        public IEnumerable<IUserHandle> UserHandles => _userHandles;

        //TODO: Populate this
        public IArchive Archive { get { throw new NotImplementedException(); } }
        public IProject Project { get { throw new NotImplementedException(); } }
        public IMachine[] Machines { get { throw new NotImplementedException(); } }
        public IIssue[] Issues { get { throw new NotImplementedException(); } }
        public IIssueType[] IssueTypes { get { throw new NotImplementedException(); } }
    }

    internal class Project : IProject
    {
        private readonly Application[] _applications;
        private readonly Session[] _sessions;
        private readonly User[] _users;
        private readonly UserHandler[] _userHandles;

        internal Project(ProjectResponse projectResponse)
        {
            Name = projectResponse.Name;
            Info = projectResponse.Info;
            _users = projectResponse.Users.Select(x => new User(x.UserName)).ToArray();
            _userHandles = projectResponse.UserHandles.Select(x => new UserHandler(x.Name)).ToArray();
            _sessions = projectResponse.Sessions.Select(x => new Session(x, this, _users.Single(y => y.Name == x.UserName), string.IsNullOrEmpty(x.UserHandleName) ? null : _userHandles.Single(y => y.Name == x.UserHandleName))).ToArray();
            _applications = projectResponse.Applications.Select(x => new Application(this, x, projectResponse.Versions, projectResponse.IssueTypes, projectResponse.Issues, projectResponse.Sessions)).ToArray();
        }

        public string Name { get; }
        public IProjectInfo Info { get; }
        public IEnumerable<IApplication> Applications => _applications;
        public IEnumerable<ISession> Sessions => _sessions;
        public IEnumerable<IUser> Users => _users;
        public IEnumerable<IUserHandle> UserHandles => _userHandles;

        //TODO: Populate this
        public IMachine[] Machines { get { throw new NotImplementedException(); } }
        public IIssue[] Issues { get { throw new NotImplementedException(); } }
        public IIssueType[] IssueTypes { get { throw new NotImplementedException(); } }
        public IVersion[] Versions { get { throw new NotImplementedException(); } }
    }

    internal class ProjectInfo : IProjectInfo
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public int VersionCount { get; set; }
        public int SessionCount { get; set; }
        public int IssueTypeCount { get; set; }
        public int IssueCount { get; set; }
        public string DashboardColor { get; set; }
    }
}
