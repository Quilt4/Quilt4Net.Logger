using System;
using Quilt4Net.Core.Exceptions;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Handlers.Configuration
{
    internal abstract class TargetConfigurationBase : ITargetConfiguration
    {
        protected readonly IConfiguration _configuration;
        protected string _location;
        protected TimeSpan? _timeout;

        protected TargetConfigurationBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual string Location
        {
            get
            {
                if (_location != null) return _location;

                //If there is no setting, read from config file to populate the value
                lock (ConfigurationBase.SyncRoot)
                {
                    if (_location == null)
                    {
                        _location = "https://www.Quilt4.com/";
                        if (!_location.EndsWith("/")) _location += "/";
                    }
                }

                return _location;
            }

            set
            {
                if (value == null) throw new ExpectedIssues(_configuration).GetException(ExpectedIssues.CannotSetLocation);
                _location = value;
                if (!_location.EndsWith("/")) _location += "/";
            }
        }

        public virtual TimeSpan Timeout
        {
            get
            {
                if (_timeout != null) return _timeout.Value;

                //If there is no setting, read from config file to populate the value
                lock (ConfigurationBase.SyncRoot)
                {
                    if (_timeout == null)
                    {
                        _timeout = new TimeSpan(0, 0, 100);
                    }
                }

                return _timeout.Value;
            }

            set
            {
                if (value == null) throw new ExpectedIssues(_configuration).GetException(ExpectedIssues.CannotSetTimeout);
                _timeout = value;
            }
        }
    }
}