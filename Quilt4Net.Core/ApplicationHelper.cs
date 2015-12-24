using System;
using System.Reflection;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal abstract class ApplicationHelper : IApplicationHelper
    {
        protected readonly IConfiguration Configuration;
        protected Assembly FirstAssembly;

        protected ApplicationHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected virtual Assembly GetFirstAssembly()
        {
            if (FirstAssembly == null) throw new InvalidOperationException("No first assembly has been set.");
            return FirstAssembly;
        }

        public virtual void SetFirstAssembly(Assembly firstAssembly)
        {
            if (FirstAssembly != null) throw new InvalidOperationException("Cannot change first assembly once it has been set.");
            FirstAssembly = firstAssembly;
        }

        protected abstract DateTime? GetBuildTime();

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

            var fingerPrint = $"AI1:{$"{applicationName}{applicationVersion}{supportToolkitNameVersion}{projectApiKey}{buildTime}".ToMd5Hash()}";

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
        
        protected virtual string GetSupportToolkitNameVersion()
        {
            var currentAssembly = typeof(ApplicationHelper).GetTypeInfo().Assembly;
            var toolkitName = currentAssembly.GetName();
            return $"{toolkitName.Name} {toolkitName.Version}";
        }

        private string GetApplicationName()
        {
            if (!string.IsNullOrEmpty(Configuration.ApplicationName))
            {
                return Configuration.ApplicationName;
            }
            return GetFirstAssembly().GetName().Name;
        }

        protected virtual bool IsClickOnce => false;

        protected virtual string GetApplicationVersion()
        {
            var assemblyVersion = GetFirstAssembly().GetName().Version;
            var applicationVersion = assemblyVersion;
            return applicationVersion.ToString();
        }
    }
}