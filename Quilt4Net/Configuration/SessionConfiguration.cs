using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    internal class SessionConfiguration : SessionConfigurationBase
    {
        public SessionConfiguration(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override string Environment
        {
            get
            {
                if (_environment != null) return _environment;

                //If there is no setting, read from config file to populate the value
                lock (ConfigurationBase.SyncRoot)
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
                if (value == null) throw new ExpectedIssues(_configuration).GetException(ExpectedIssues.CannotSetEnvironment);
                _environment = value;
            }
        }
    }
}