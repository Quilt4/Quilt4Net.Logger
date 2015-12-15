using System;
using System.Reflection;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal abstract class ApplicationHelper : IApplicationHelper
    {
        protected readonly IConfiguration Configuration;
        protected Assembly _firstAssembly;

        public ApplicationHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected abstract Assembly GetFirstAssembly();
        protected abstract DateTime? GetBuildTime();

        protected string GetProjectApiKey()
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
            return string.Format("{0} {1}", toolkitName.Name, toolkitName.Version);
        }

        private string GetApplicationName()
        {
            if (!string.IsNullOrEmpty(Configuration.ApplicationName))
            {
                return Configuration.ApplicationName;
            }
            return GetFirstAssembly().GetName().Name;
        }

        protected virtual bool IsClickOnce { get { return false; } }

        protected virtual string GetApplicationVersion()
        {
            var assemblyVersion = GetFirstAssembly().GetName().Version;
            var clickOnceVersion = (Version)null;
            if (IsClickOnce)
            {
                throw new NotSupportedException();
            }
            var applicationVersion = clickOnceVersion ?? assemblyVersion;
            return applicationVersion.ToString();
        }

        //private void SetFirstAssembly(Assembly firstAssembly)
        //{
        //    _firstAssembly = firstAssembly;
        //}

        //private Assembly GetFirstAssembly()
        //{
        //    if (_firstAssembly == null)
        //    {
        //        lock (_syncRoot)
        //        {
        //            if (_firstAssembly == null)
        //            {
        //                //_firstAssembly = Assembly.GetEntryAssembly(); //TODO: This only works for regular .NET assemblies
        //                if (_firstAssembly == null) throw ExpectedIssues.GetException(ExpectedIssues.CannotAutomaticallyRetrieveAssembly);
        //            }
        //        }
        //    }

        //    return _firstAssembly;
        //}
    }
}