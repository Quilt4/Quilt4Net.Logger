using Quilt4Net.Core;

namespace Quilt4Net
{
    internal class SessionConfiguration : Core.SessionConfiguration
    {
        public override string Environment
        {
            get
            {
                if (_environment != null) return _environment;

                //If there is no setting, read from config file to populate the value
                lock (Core.Configuration.SyncRoot)
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
                if (value == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotSetEnvironment);
                _environment = value;
            }
        }
    }
}