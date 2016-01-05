using System.Configuration;

namespace Quilt4Net.Configuration
{
    internal class SessionElement : ConfigurationElement
    {
        private SessionElement()
        {
        }

        [ConfigurationProperty("Environment", IsRequired = false)]
        public string Environment
        {
            get { return (string)base[new ConfigurationProperty("Environment", typeof(string), null)]; }
            set { this["Environment"] = value; }
        }
    }
}