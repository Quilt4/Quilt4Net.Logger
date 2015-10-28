using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Quilt4Net.DataTransfer;
using Tharga.Quilt4Net.Interfaces;

namespace Tharga.Quilt4Net.Entities
{
    internal class Machine : IMachine
    {
        public Machine(string machineKey)
        {
            MachineKey = machineKey;
        }

        public string MachineKey { get; }

        //TODO: Populate this
        public string Name { get { throw new NotImplementedException(); } }
        public IDictionary<string, string> Data { get { throw new NotImplementedException(); } }
        public ISession[] Sessions { get { throw new NotImplementedException(); } }
        public IUser[] Users { get { throw new NotImplementedException(); } }
        public IUserHandle[] UserHandles { get { throw new NotImplementedException(); } }
        public IIssue[] Issues { get { throw new NotImplementedException(); } }
        public IIssueType[] IssueTypes { get { throw new NotImplementedException(); } }
        public IVersion[] Versions { get { throw new NotImplementedException(); } }
        public IApplication[] Applications { get { throw new NotImplementedException(); } }
        public IProject[] Projects { get { throw new NotImplementedException(); } }
    }

    internal class User : IUser
    {
        public User(string userKey, string name)
        {
            UserKey = userKey;
            Name = name;
        }

        public string UserKey { get; }
        public string Name { get; }

        //TODO: Populate this
        public ISession[] Sessions { get { throw new NotImplementedException(); } }
        public IMachine[] Users { get { throw new NotImplementedException(); } }
        public IIssue[] Issues { get { throw new NotImplementedException(); } }
        public IIssueType[] IssueTypes { get { throw new NotImplementedException(); } }
        public IVersion[] Versions { get { throw new NotImplementedException(); } }
        public IApplication[] Applications { get { throw new NotImplementedException(); } }
    }

    internal class Project : IProject
    {
        private readonly Application[] _applications;
        private readonly Session[] _sessions;
        private readonly User[] _users;
        private readonly UserHandler[] _userHandles;
        private readonly Machine[] _machines;

        internal Project(ProjectResponse projectResponse)
        {
            Name = projectResponse.Name;
            Info = projectResponse.Info;
            _users = projectResponse.Users.Select(x => new User(x.UserKey, x.UserName)).ToArray();
            _userHandles = projectResponse.UserHandles.Select(x => new UserHandler(x.Name)).ToArray();
            _machines = projectResponse.Machines.Select(x => new Machine(x.MachineKey)).ToArray();
            _sessions = projectResponse.Sessions.Select(x => new Session(x, this, _users.Single(y => y.UserKey == x.UserKey), string.IsNullOrEmpty(x.UserHandleName) ? null : _userHandles.Single(y => y.Name == x.UserHandleName), _machines.Single(y => y.MachineKey == x.MachineKey ))).ToArray();
            _applications = projectResponse.Applications.Select(x => new Application(this, x, projectResponse.Versions, projectResponse.IssueTypes, projectResponse.Issues, projectResponse.Sessions)).ToArray();
        }

        public string Name { get; }
        public IProjectInfo Info { get; }
        public IEnumerable<IApplication> Applications => _applications;
        public IEnumerable<ISession> Sessions => _sessions;
        public IEnumerable<IUser> Users => _users;
        public IEnumerable<IUserHandle> UserHandles => _userHandles;
        public IEnumerable<IMachine> Machines => _machines;
        public IEnumerable<IIssue> Issues { get { return _applications.SelectMany(x => x.Issues); } }
        public IEnumerable<IIssueType> IssueTypes { get { return _applications.SelectMany(x => x.IssueTypes); } }
        public IEnumerable<IVersion> Versions { get { return _applications.SelectMany(x => x.Versions); } }
    }
}
