using System;
using Quilt4Net.Core;

namespace Quilt4Net
{
    internal class TargetConfiguration : Core.TargetConfiguration
    {
        public override string Location
        {
            get
            {
                if (_location != null) return _location;

                //If there is no setting, read from config file to populate the value
                lock (Core.Configuration.SyncRoot)
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
                if (value == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotSetLocation);
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
                lock (Core.Configuration.SyncRoot)
                {
                    if (_timeout == null)
                    {
                        _timeout = ConfigSection.Instance.TargetValue.Timeout;
                    }
                }

                return _timeout.Value;
            }

            set
            {
                if (value == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotSetTimeout);
                _timeout = value;
            }
        }
    }
}