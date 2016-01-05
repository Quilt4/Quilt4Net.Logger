using Quilt4Net.Core.Exceptions;
using Quilt4Net.Core.Handlers.Configuration;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Configuration
{
    internal class SessionConfiguration : SessionConfigurationBase
    {
        public SessionConfiguration(IConfigurationHandler configurationHandler)
            : base(configurationHandler)
        {
        }

        public override string Environment
        {
            get
            {
                if (_environment != null) return _environment;

                //If there is no setting, read from config file to populate the value
                lock (ConfigurationHandlerBase.SyncRoot)
                {
                    if (_environment == null)
                    {
                        _environment = ConfigSection.Instance.SessionValue.Environment ?? string.Empty;
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