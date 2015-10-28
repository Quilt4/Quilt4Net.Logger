using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Session : ISession
    {
        public Session(SessionResponse sessionResponse)
        {
            
        }


        public Guid SessionKey { get; }
        public DateTime StartTime { get; }
        public DateTime? EndTime { get; }
        public IEnvironment Environment { get; }
        public string CallerIpAddress { get; }
        public TimeSpan Duration { get; }
        public IUser User { get; }
        public IUserHandle UserHandle { get; }
        public IMachine Machine { get; }
        public IIssue[] Issues { get; }
        public IIssueType[] IssueTypes { get; }
        public IVersion Version { get; }
        public IApplication Application { get; }
        public IProject Project { get; }
    }

    internal class Issue : IIssue
    {
        public Issue(IIssueType issueType, ISession session, IssueResponse issueResponse)
        {
            IssueTime = issueResponse.IssueTime;
            //LinkedIssues { get; } //TODO: Handle
            Data = issueResponse.Data;
            UserInput = issueResponse.UserInput;
            Visible = issueResponse.Visible;
            Session = session;
            IssueType = issueType;
        }

        public DateTime IssueTime { get; }
        public IIssue LinkedIssues { get; }
        public IDictionary<string, string> Data { get; }
        public string UserInput { get; }
        public bool? Visible { get; }
        public ISession Session { get; }
        public IIssueType IssueType { get; }
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
        private readonly Session[] _sessions;

        public IssueType(IVersion version, IssueTypeResponse issueTypeResponse, IEnumerable<IssueResponse> issueResponses, IEnumerable<SessionResponse> sessionResponses)
        {
            Message = issueTypeResponse.Message;
            StackTrace = new StackTrace(issueTypeResponse.StackTrace);
            Type = issueTypeResponse.Type;
            ResponseMessage = issueTypeResponse.ResponseMessage;
            Ticket = issueTypeResponse.Ticket;
            Level = new Issuelevel(issueTypeResponse.Level);
            //InnerIssue //TODO: Fix!
            var sessions = sessionResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == version.Name).Select(x => new Session(x)).ToArray(); //Pick all sessions for this application and version
            var session = sessions.Single(y => y.SessionKey == issueTypeResponse.SessionKey);
            _issues = issueResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == version.Name && x.IssueTypeMessage == Message).Select(x => new Issue(this, session, x)).ToArray();
            _sessions = sessions.Where(x => _issues.Any(y => y.Session.SessionKey == x.SessionKey)).ToArray(); //Pick just the sessions where this issue type has appeared
        }

        public string Message { get; }
        public IStackTrace StackTrace { get; }
        public string Type { get; }
        public string ResponseMessage { get; }
        public int Ticket { get; }
        public IIssuelevel Level { get; }
        public IIssueType InnerIssue { get; }
        public IEnumerable<IIssue> Issues => _issues;
        public IEnumerable<ISession> Sessions => _sessions;

        //TODO: Populate this
        public IVersion Version { get; }
        public IApplication Application { get; }
        public IProject Project { get; }
        public IUser[] Users { get; }
        public IUserHandle[] UserHandles { get; }
        public IMachine[] Machines { get; }
    }

    internal class Version : IVersion
    {
        private readonly IssueType[] _issueTypes;
        private readonly IssueResponse[] _issueResponses;
        private readonly SessionResponse[] _sessionResponses;

        public Version(IApplication application, VersionResponse versionResponse, IEnumerable<IssueTypeResponse> issueTypeResponses, IEnumerable<IssueResponse> issueResponses, IEnumerable<SessionResponse> sessionResponses)
        {
            Application = application;
            Name = versionResponse.Name;
            BuildTime = versionResponse.BuildTime;
            SupportToolkit = versionResponse.SupportToolkit;
            _issueResponses = issueResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == Name).ToArray();
            _sessionResponses = sessionResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == Name).ToArray();
            _issueTypes = issueTypeResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == Name).Select(x => new IssueType(this, x, _issueResponses, _sessionResponses)).ToArray();
        }

        public string Name { get; }
        public DateTime? BuildTime { get; }
        public string SupportToolkit { get; }
        public IApplication Application { get; }
        public IEnumerable<IIssueType> IssueTypes => _issueTypes;

        //TODO: Populate this
        public IProject Project { get; }
        public ISession[] Sessions { get; }
        public IUser[] Users { get; }
        public IUserHandle[] UserHandles { get; }
        public IMachine[] Machines { get; }
        public IIssue[] Issues { get; }
    }

    internal class Application : IApplication
    {
        private readonly Version[] _versions;
        private readonly IssueTypeResponse[] _issueTypeResponses;
        private readonly IssueResponse[] _issueResponses;
        private readonly SessionResponse[] _sessionResponses;

        internal Application(ApplicationResponse applicationResponse, IEnumerable<VersionResponse> versionResponses, IEnumerable<IssueTypeResponse> issueTypeResponses, IEnumerable<IssueResponse> issueResponses, IEnumerable<SessionResponse> sessionResponses)
        {
            Name = applicationResponse.Name;
            _issueTypeResponses = issueTypeResponses.Where(x => x.ApplicationName == Name).ToArray();
            _issueResponses = issueResponses.Where(x => x.ApplicationName == Name).ToArray();
            _sessionResponses = sessionResponses.Where(x => x.ApplicationName == Name).ToArray();
            _versions = versionResponses.Where(x => x.ApplicationName == Name).Select(x => new Version(this, x, _issueTypeResponses, _issueResponses, _sessionResponses)).ToArray();
        }

        public string Name { get; }
        public IEnumerable<IVersion> Versions => _versions;

        //TODO: Populate this
        public IArchive Archive { get; }
        public IProject Project { get; }
        public ISession[] Sessions { get; }
        public IUser[] Users { get; }
        public IUserHandle[] UserHandles { get; }
        public IMachine[] Machines { get; }
        public IIssue[] Issues { get; }
        public IIssueType[] IssueTypes { get; }
    }

    internal class Project : IProject
    {
        private readonly Application[] _applications;

        internal Project(ProjectResponse projectResponse)
        {
            Name = projectResponse.Name;
            Info = projectResponse.Info;
            _applications = projectResponse.Applications.Select(x => new Application(x, projectResponse.Versions, projectResponse.IssueTypes, projectResponse.Issues, projectResponse.Sessions)).ToArray();
        }

        public string Name { get; }
        public IProjectInfo Info { get; }
        public IEnumerable<IApplication> Applications => _applications;

        //TODO: Populate this
        public ISession[] Sessions { get { throw new NotImplementedException(); } }
        public IUser[] Users { get { throw new NotImplementedException(); } }
        public IUserHandle[] UserHandles { get { throw new NotImplementedException(); } }
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
