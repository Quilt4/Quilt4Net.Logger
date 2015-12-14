using System;
using System.Configuration;

namespace Quilt4Net
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
        public TimeSpan Timeout
        {
            get
            {
                var configurationProperty = new ConfigurationProperty("Timeout", typeof(TimeSpan?), null);
                return (TimeSpan?)base[configurationProperty] ?? new TimeSpan(0, 0, 60);
            }

            set { this["Timeout"] = value; }
        }
    }
}