using System;
using System.Reflection;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal abstract class ApplicationInformationBase : IApplicationInformation
    {
        private readonly IHashHandler _hashHandler;
        protected readonly IConfiguration Configuration;
        protected ApplicationNameVersion ApplicationNameVersion;

        protected ApplicationInformationBase(IConfiguration configuration, IHashHandler hashHandler)
        {
            _hashHandler = hashHandler;
            Configuration = configuration;
        }

        protected virtual bool IsClickOnce => false;

        public void SetApplicationNameVersion(ApplicationNameVersion applicationNameVersion)
        {
            if (ApplicationNameVersion != null) throw new InvalidOperationException("Cannot change the application name version once it has been set.");
            ApplicationNameVersion = applicationNameVersion;
        }

        public virtual ApplicationNameVersion GetApplicationNameVersion()
        {
            if (ApplicationNameVersion == null) throw new InvalidOperationException("No version name as been assigned. Call the SetApplicationNameVersion function before this operation");
            return ApplicationNameVersion;
        }

        protected abstract DateTime? GetBuildTime();

        protected virtual string GetFingerPrint()
        {
            return GetFingerPrint(GetApplicationName(), GetApplicationVersion(), GetSupportToolkitNameVersion(), GetProjectApiKey(), GetBuildTime());
        }

        private string GetProjectApiKey()
        {
            return Configuration.ProjectApiKey;
        }

        public ApplicationData GetApplicationData()
        {
            var applicationName = GetApplicationName();
            var applicationVersion = GetApplicationVersion();
            var supportToolkitNameVersion = GetSupportToolkitNameVersion();
            var buildTime = GetBuildTime();
            var projectApiKey = GetProjectApiKey();

            var fingerPrint = GetFingerPrint(applicationName, applicationVersion, supportToolkitNameVersion, projectApiKey, buildTime);

            var application = new ApplicationData
            {
                Fingerprint = fingerPrint,
                Name = applicationName,
                BuildTime = buildTime,
                SupportToolkitNameVersion = supportToolkitNameVersion,
                Version = applicationVersion,
            };

            return application;
        }

        private string GetFingerPrint(string applicationName, string applicationVersion, string supportToolkitNameVersion, string projectApiKey, DateTime? buildTime)
        {
            return $"AI1:{_hashHandler.ToMd5Hash($"{applicationName}{applicationVersion}{supportToolkitNameVersion}{projectApiKey}{buildTime}")}";
        }

        protected virtual string GetSupportToolkitNameVersion()
        {
            var currentAssembly = typeof(ApplicationInformationBase).GetTypeInfo().Assembly;
            var toolkitName = currentAssembly.GetName();
            return $"{toolkitName.Name} {toolkitName.Version}";
        }

        protected virtual string GetApplicationName()
        {
            if (!string.IsNullOrEmpty(Configuration.ApplicationName))
            {
                return Configuration.ApplicationName;
            }
            return GetApplicationNameVersion().Name;
        }

        protected virtual string GetApplicationVersion()
        {
            if (!string.IsNullOrEmpty(Configuration.ApplicationVersion))
            {
                return Configuration.ApplicationVersion;
            }
            return GetApplicationNameVersion().Version;
        }
    }
}