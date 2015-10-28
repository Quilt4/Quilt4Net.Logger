using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Version : IVersion
    {
        public Version(IApplication application, VersionResponse versionResponse)
        {
            Application = application;
            Name = versionResponse.Name;
            BuildTime = versionResponse.BuildTime;
            SupportToolkit = versionResponse.SupportToolkit;
        }

        public string Name { get; }
        public DateTime? BuildTime { get; }
        public string SupportToolkit { get; }
        public IApplication Application { get; }

        //TODO: Populate this
        public IIssueType[] IssueTypes { get; }
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

        internal Application(ApplicationResponse applicationResponse, IEnumerable<VersionResponse> versionResponses)
        {
            Name = applicationResponse.Name;
            _versions = versionResponses.Where(x => x.ApplicationName == Name).Select(x => new Version(this, x)).ToArray();
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
            _applications = projectResponse.Applications.Select(x => new Application(x, projectResponse.Versions)).ToArray();
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
