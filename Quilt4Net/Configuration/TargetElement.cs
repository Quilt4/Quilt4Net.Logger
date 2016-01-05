using System.Configuration;

namespace Quilt4Net.Configuration
{
    internal class TargetElement : ConfigurationElement
    {
        private TargetElement()
        {
        }

        [ConfigurationProperty("Location", IsRequired = false)]
        public string Location
        {
            get
            {
                var configurationProperty = new ConfigurationProperty("Location", typeof(string), null);
                return (string)base[configurationProperty] ?? "https://www.Quilt4.com/";
            }

            set { this["Location"] = value; }
        }

        [ConfigurationProperty("Timeout", IsRequired = false)]
        public int Timeout
        {
            get
            {
                var configurationProperty = new ConfigurationProperty("Timeout", typeof(int?), null);
                return (int?)base[configurationProperty] ?? 60;
            }

            set { this["Timeout"] = value; }
        }
    }
}