using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class IssueType : IIssueType
    {
        public IssueType()
        {
        }

        public string Message { get; }
        public string Type { get; }
        public string ResponseMessage { get; }
        public int Ticket { get; }
        public IIssuelevel Level { get; }
        public IIssueType InnerIssue { get; }
        public IIssue[] Issues { get; }
        public IVersion Version { get; }
        public IApplication Application { get; }
        public IProject Project { get; }
        public ISession[] Sessions { get; }
        public IUser[] Users { get; }
        public IUserHandle[] UserHandles { get; }
        public IMachine[] Machines { get; }
    }

    internal class Version : IVersion
    {
        private readonly IssueType[] _issueTypes;

        public Version(IApplication application, VersionResponse versionResponse, IEnumerable<IssueTypeResponse> issueTypeResponses)
        {
            Application = application;
            Name = versionResponse.Name;
            BuildTime = versionResponse.BuildTime;
            SupportToolkit = versionResponse.SupportToolkit;
            _issueTypes = issueTypeResponses.Where(x => x.ApplicationName == Application.Name && x.VersionName == Name).Select(x => new IssueType()).ToArray();
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

        internal Application(ApplicationResponse applicationResponse, IEnumerable<VersionResponse> versionResponses, IEnumerable<IssueTypeResponse> issueTypeResponses)
        {
            Name = applicationResponse.Name;
            _issueTypeResponses = issueTypeResponses.Where(x => x.ApplicationName == Name).ToArray();
            _versions = versionResponses.Where(x => x.ApplicationName == Name).Select(x => new Version(this, x, _issueTypeResponses)).ToArray();
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
            _applications = projectResponse.Applications.Select(x => new Application(x, projectResponse.Versions, projectResponse.IssueTypes)).ToArray();
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
}
