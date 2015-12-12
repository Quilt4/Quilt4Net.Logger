using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal abstract class SessionConfiguration : ISessionConfiguration
    {
        protected string _environment;

        public virtual string Environment
        {
            get
            {
                if (_environment != null) return _environment;

                //If there is no setting, read from config file to populate the value
                lock (Configuration.SyncRoot)
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
                if (value == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotSetEnvironment);
                _environment = value;
            }
        }
    }
}