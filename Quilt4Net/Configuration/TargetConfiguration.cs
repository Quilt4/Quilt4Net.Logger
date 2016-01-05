using System;
using Quilt4Net.Core.Exceptions;
using Quilt4Net.Core.Handlers.Configuration;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Configuration
{
    internal class TargetConfiguration : TargetConfigurationBase
    {
        public TargetConfiguration(IConfigurationHandler configurationHandler)
            : base(configurationHandler)
        {
        }

        public override string Location
        {
            get
            {
                if (_location != null) return _location;

                //If there is no setting, read from config file to populate the value
                lock (ConfigurationHandlerBase.SyncRoot)
                {
                    if (_location == null)
                    {
                        _location = ConfigSection.Instance.TargetValue.Location ?? string.Empty;
                        if (!_location.EndsWith("/")) _location += "/";
                    }
                }

                return _location;
            }

            set
            {
                if (value == null) throw new ExpectedIssues(_configurationHandler).GetException(ExpectedIssues.CannotSetLocation);
                _location = value;
                if (!_location.EndsWith("/")) _location += "/";
            }
        }

        public override TimeSpan Timeout
        {
            get
            {
                if (_timeout != null) return _timeout.Value;

                //If there is no setting, read from config file to populate the value
                lock (ConfigurationHandlerBase.SyncRoot)
                {
                    if (_timeout == null)
                    {
                        _timeout = new TimeSpan(0, 0, 0, ConfigSection.Instance.TargetValue.Timeout);
                    }
                }

                return _timeout.Value;
            }

            set
            {
                if (value == null) throw new ExpectedIssues(_configurationHandler).GetException(ExpectedIssues.CannotSetTimeout);
                _timeout = value;
            }
        }
    }
}