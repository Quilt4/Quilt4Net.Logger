using System;
using System.Configuration;

namespace Quilt4Net
{
    internal class ConfigSection : ConfigurationSection
    {
        private static readonly Lazy<ConfigSection> _instance = new Lazy<ConfigSection>(() => ConfigurationManager.GetSection("Tharga/Quilt4Net") as ConfigSection ?? new ConfigSection());

        private ConfigSection()
        {
        }

        public static ConfigSection Instance
        {
            get { return _instance.Value; }
        }

        [ConfigurationProperty("ProjectApiKey", IsRequired = false)]
        public string ProjectApiKeyValue
        {
            get
            {
                var value = base[new ConfigurationProperty("ProjectApiKey", typeof(string), null)];
                return (string)value;
            }

            set { this["ProjectApiKey"] = value; }
        }

        [ConfigurationProperty("ApplicationName", IsRequired = false)]
        public string ApplicationNameValue
        {
            get
            {
                var value = base[new ConfigurationProperty("ApplicationName", typeof(string), null)];
                return (string)value;
            }

            set { this["ApplicationName"] = value; }
        }

        [ConfigurationProperty("ApplicationVersion", IsRequired = false)]
        public string ApplicationVersionValue
        {
            get
            {
                var value = base[new ConfigurationProperty("ApplicationVersion", typeof(string), null)];
                return (string)value;
            }

            set { this["ApplicationVersion"] = value; }
        }

        [ConfigurationProperty("Enabled", IsRequired = false, DefaultValue = true)]
        public bool EnabledValue
        {
            get { return (bool)this["Enabled"]; }
            set { this["Enabled"] = value; }
        }

        [ConfigurationProperty("UseBuildTime", IsRequired = false, DefaultValue = false)]
        public bool UseBuildTimeValue
        {
            get { return (bool)this["UseBuildTime"]; }
            set { this["UseBuildTime"] = value; }
        }

        [ConfigurationProperty("Session")]
        public SessionElement SessionValue
        {
            get { return (SessionElement)this["Session"]; }
            set { this["Session"] = value; }
        }

        [ConfigurationProperty("Target")]
        internal TargetElement TargetValue
        {
            get { return (TargetElement)this["Target"]; }
            set { this["Target"] = value; }
        }
    }
}