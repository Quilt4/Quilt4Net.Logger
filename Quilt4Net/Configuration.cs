using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    internal class Configuration : Core.Configuration
    {
        internal Configuration()
        {
            Session = new SessionConfiguration();
            Target = new TargetConfiguration();
        }

        public override bool Enabled
        {
            get
            {
                if (_enabled != null) return _enabled.Value;

                //If there is no setting, read from config file to populate the value
                lock (SyncRoot)
                {
                    if (_enabled == null)
                    {
                        _enabled = ConfigSection.Instance.EnabledValue;
                    }
                }

                return _enabled.Value;
            }

            set { _enabled = value; }
        }

        public override string ProjectApiKey
        {
            get
            {
                if (_projectApiKey != null) return _projectApiKey;

                //If there is no setting, read from config file to populate the value
                lock (SyncRoot)
                {
                    if (_projectApiKey == null)
                    {
                        _projectApiKey = ConfigSection.Instance.ProjectApiKeyValue;
                    }
                }

                return _projectApiKey;
            }

            set
            {
                if (value == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotSetProjectApiKey);
                _projectApiKey = value;
            }
        }

        internal override string ApplicationName
        {
            get
            {
                if (_applicationName != null) return _applicationName;

                //If there is no setting, read from config file to populate the value
                lock (SyncRoot)
                {
                    if (_applicationName == null)
                    {
                        _applicationName = ConfigSection.Instance.ApplicationNameValue;
                    }
                }

                return _applicationName;
            }

            set { _applicationName = value; }
        }

        internal override string ApplicationVersion
        {
            get
            {
                if (_applicationVersion != null) return _applicationVersion;

                //If there is no setting, read from config file to populate the value
                lock (SyncRoot)
                {
                    if (_applicationVersion == null)
                    {
                        _applicationVersion = ConfigSection.Instance.ApplicationVersionValue;
                    }
                }

                return _applicationVersion;
            }

            set { _applicationVersion = value; }
        }

        public override bool UseBuildTime
        {
            get
            {
                if (_useBuildTime != null) return _useBuildTime.Value;

                //If there is no setting, read from config file to populate the value
                lock (SyncRoot)
                {
                    if (_useBuildTime == null)
                    {
                        _useBuildTime = ConfigSection.Instance.UseBuildTimeValue;
                    }
                }

                return _useBuildTime.Value;
            }

            set { _useBuildTime = value; }
        }

        public override ISessionConfiguration Session { get; }

        public override ITargetConfiguration Target { get; }
    }
}