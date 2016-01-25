using System;
using System.Deployment.Application;
using System.IO;
using System.Reflection;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    internal class ApplicationInformation : ApplicationInformationBase
    {
        private readonly object _syncRoot = new object();
        private Assembly _firstAssembly;

        internal ApplicationInformation(IConfiguration configuration, IHashHandler hashHandler)
            : base(configuration, hashHandler)
        {
        }

        public override ApplicationNameVersion GetApplicationNameVersion()
        {
            if (ApplicationNameVersion == null)
            {
                lock (_syncRoot)
                {
                    if (ApplicationNameVersion == null)
                    {
                        var firstAssembly = GetFirstAssembly();
                        ApplicationNameVersion = new ApplicationNameVersion(firstAssembly.GetName().Name, firstAssembly.GetName().Version.ToString());
                        if (ApplicationNameVersion == null) throw new ExpectedIssues(Configuration).GetException(ExpectedIssues.CannotAutomaticallyRetrieveAssembly);
                    }
                }
            }

            return ApplicationNameVersion;
        }

        private Assembly GetFirstAssembly()
        {
            if (_firstAssembly == null)
            {
                lock (_syncRoot)
                {
                    if (_firstAssembly == null)
                    {
                        _firstAssembly = Assembly.GetEntryAssembly();
                        if (_firstAssembly == null) throw new ExpectedIssues(Configuration).GetException(ExpectedIssues.CannotAutomaticallyRetrieveAssembly);
                    }
                }
            }

            return _firstAssembly;
        }

        protected internal void SetFirstAssembly(Assembly firstAssembly)
        {
            if (_firstAssembly != null && !ReferenceEquals(firstAssembly, _firstAssembly)) throw new InvalidOperationException("Cannot change the first assembly once it has been assigned.");
            _firstAssembly = firstAssembly;
        }

        protected override DateTime? GetBuildTime()
        {
            if (!Configuration.UseBuildTime) return null;

            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;

            FileStream s = null;
            var b = new byte[2048];

            try
            {
                var filePath = GetFirstAssembly().Location;
                s = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                s?.Close();
            }

            var i = BitConverter.ToInt32(b, peHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(b, i + linkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);

            return dt;
        }

        protected override string GetSupportToolkitNameVersion()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var toolkitName = currentAssembly.GetName();
            return $"{toolkitName.Name} {toolkitName.Version}";
        }

        protected override bool IsClickOnce => ApplicationDeployment.IsNetworkDeployed;

        protected override string GetApplicationName()
        {
            return base.GetApplicationName();
        }

        protected override string GetApplicationVersion()
        {
            var version = base.GetApplicationVersion();
            var clickOnceVersion = (string)null;
            if (IsClickOnce)
            {
                clickOnceVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            var applicationVersion = clickOnceVersion ?? version;
            return applicationVersion;
        }
    }
}