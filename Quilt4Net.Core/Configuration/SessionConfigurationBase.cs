using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal abstract class SessionConfigurationBase : ISessionConfiguration
    {
        protected readonly IConfiguration _configuration;
        protected string _environment;

        protected SessionConfigurationBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual string Environment
        {
            get
            {
                if (_environment != null) return _environment;

                //If there is no setting, read from config file to populate the value
                lock (ConfigurationBase.SyncRoot)
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
                if (value == null) throw new ExpectedIssues(_configuration).GetException(ExpectedIssues.CannotSetEnvironment);
                _environment = value;
            }
        }
    }
}