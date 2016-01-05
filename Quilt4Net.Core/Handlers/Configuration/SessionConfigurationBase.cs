using Quilt4Net.Core.Exceptions;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Handlers.Configuration
{
    internal abstract class SessionConfigurationBase : ISessionConfiguration
    {
        protected readonly IConfigurationHandler _configurationHandler;
        protected string _environment;

        protected SessionConfigurationBase(IConfigurationHandler configurationHandler)
        {
            _configurationHandler = configurationHandler;
        }

        public virtual string Environment
        {
            get
            {
                if (_environment != null) return _environment;

                //If there is no setting, read from config file to populate the value
                lock (ConfigurationHandlerBase.SyncRoot)
                {
                    if (_environment == null)
                    {
                        _environment = string.Empty;
                    }
                }

                return _environment;
            }

            set
            {
                if (value == null) throw new ExpectedIssues(_configurationHandler).GetException(ExpectedIssues.CannotSetEnvironment);
                _environment = value;
            }
        }
    }
}