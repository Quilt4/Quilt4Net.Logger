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

        internal ApplicationInformation(IConfiguration configuration, IHashHandler hashHandler)
            : base(configuration, hashHandler)
        {
        }

        protected override Assembly GetFirstAssembly()
        {
            if (FirstAssembly == null)
            {
                lock (_syncRoot)
                {
                    if (FirstAssembly == null)
                    {
                        FirstAssembly = Assembly.GetEntryAssembly();
                        if (FirstAssembly == null) throw new ExpectedIssues(Configuration).GetException(ExpectedIssues.CannotAutomaticallyRetrieveAssembly);
                    }
                }
            }

            return FirstAssembly;
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

        protected override string GetApplicationVersion()
        {
            var assemblyVersion = GetFirstAssembly().GetName().Version;
            var clickOnceVersion = (Version)null;
            if (IsClickOnce)
            {
                clickOnceVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            var applicationVersion = clickOnceVersion ?? assemblyVersion;
            return applicationVersion.ToString();
        }
    }
}