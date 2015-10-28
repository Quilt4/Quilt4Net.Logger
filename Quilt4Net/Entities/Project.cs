using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
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
        public Session(SessionResponse sessionResponse, IProject project)
        {
            Project = project;
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
        public IUser User { get { throw new NotImplementedException(); } }
        public IUserHandle UserHandle { get { throw new NotImplementedException(); } }
        public IMachine Machine { get { throw new NotImplementedException(); } }
        //public IIssue[] Issues { get { throw new NotImplementedException(); } }
        //public IIssueType[] IssueTypes { get { throw new NotImplementedException(); } }
        //public IVersion Version { get { return Project.Applications.SelectMany(x => x.Versions).Single(x => x.Name == _sessionResponse.VersionName); } }
        //public IApplication Application { get { return Project.Applications.Single(x => x.Name == _sessionResponse.ApplicationName); } }
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
        public IIssue LinkedIssues { get { throw new NotImplementedException(); } }
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
        private readonly ISession[] _sessions;

        public IssueType(IProject project, IApplication application, IVersion version, IssueTypeResponse issueTypeResponse, IEnumerable<IssueResponse> issueResponses, IEnumerable<SessionResponse> sessionResponses)
        {
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

        //TODO: Populate this
        public IProject Project { get { throw new NotImplementedException(); } }
        public IUser[] Users { get { throw new NotImplementedException(); } }
        public IUserHandle[] UserHandles { get { throw new NotImplementedException(); } }
        public IMachine[] Machines { get { throw new NotImplementedException(); } }
    }

    internal class Version : IVersion
    {
        private readonly IssueType[] _issueTypes;
        private readonly IssueResponse[] _issueResponses;
        private readonly ISession[] _sessions;

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
        }

        public string Name { get; }
        public DateTime? BuildTime { get; }
        public string SupportToolkit { get; }
        public IApplication Application { get; }
        public IEnumerable<IIssueType> IssueTypes => _issueTypes;
        public IProject Project { get; }
        public IEnumerable<ISession> Sessions => _sessions;

        //TODO: Populate this
        public IUser[] Users { get { throw new NotImplementedException(); } }
        public IUserHandle[] UserHandles { get { throw new NotImplementedException(); } }
        public IMachine[] Machines { get { throw new NotImplementedException(); } }
        public IIssue[] Issues { get { throw new NotImplementedException(); } }
    }

    internal class Application : IApplication
    {
        private readonly Version[] _versions;
        private readonly IssueTypeResponse[] _issueTypeResponses;
        private readonly IssueResponse[] _issueResponses;
        private readonly ISession[] _sessions;

        internal Application(IProject project, ApplicationResponse applicationResponse, IEnumerable<VersionResponse> versionResponses, IEnumerable<IssueTypeResponse> issueTypeResponses, IEnumerable<IssueResponse> issueResponses, SessionResponse[] sessionResponses)
        {
            Name = applicationResponse.Name;
            _issueTypeResponses = issueTypeResponses.Where(x => x.ApplicationName == Name).ToArray();
            _issueResponses = issueResponses.Where(x => x.ApplicationName == Name).ToArray();
            var applicationSessionResponses = sessionResponses.Where(x => x.ApplicationName == Name).ToArray();
            _versions = versionResponses.Where(x => x.ApplicationName == Name).Select(x => new Version(project, this, x, _issueTypeResponses, _issueResponses, applicationSessionResponses)).ToArray();
            _sessions = project.Sessions.Where(x => sessionResponses.Any(y => y.ApplicationName == Name)).ToArray();
        }

        public string Name { get; }
        public IEnumerable<IVersion> Versions => _versions;
        public IEnumerable<ISession> Sessions => _sessions;

        //TODO: Populate this
        public IArchive Archive { get { throw new NotImplementedException(); } }
        public IProject Project { get { throw new NotImplementedException(); } }
        public IUser[] Users { get { throw new NotImplementedException(); } }
        public IUserHandle[] UserHandles { get { throw new NotImplementedException(); } }
        public IMachine[] Machines { get { throw new NotImplementedException(); } }
        public IIssue[] Issues { get { throw new NotImplementedException(); } }
        public IIssueType[] IssueTypes { get { throw new NotImplementedException(); } }
    }

    internal class Project : IProject
    {
        private readonly Application[] _applications;
        private readonly ISession[] _sessions;

        internal Project(ProjectResponse projectResponse)
        {
            Name = projectResponse.Name;
            Info = projectResponse.Info;
            _sessions = projectResponse.Sessions.Select(x => new Session(x, this)).ToArray();
            _applications = projectResponse.Applications.Select(x => new Application(this, x, projectResponse.Versions, projectResponse.IssueTypes, projectResponse.Issues, projectResponse.Sessions)).ToArray();
        }

        public string Name { get; }
        public IProjectInfo Info { get; }
        public IEnumerable<IApplication> Applications => _applications;
        public IEnumerable<ISession> Sessions => _sessions;

        //TODO: Populate this
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
