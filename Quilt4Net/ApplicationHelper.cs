using System;
using System.Deployment.Application;
using System.IO;
using System.Reflection;
using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    internal class ApplicationHelper : Core.ApplicationHelper
    {
        private readonly object _syncRoot = new object();

        public ApplicationHelper(IConfiguration configuration)
            : base(configuration)
        {
        }

        protected override Assembly GetFirstAssembly()
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

        protected override DateTime? GetBuildTime()
        {
            if (!Configuration.UseBuildTime) return null;

            const int PeHeaderOffset = 60;
            const int LinkerTimestampOffset = 8;

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
                if (s != null) s.Close();
            }

            var i = BitConverter.ToInt32(b, PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(b, i + LinkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);

            return dt;
        }

        protected override string GetSupportToolkitNameVersion()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var toolkitName = currentAssembly.GetName();
            return string.Format("{0} {1}", toolkitName.Name, toolkitName.Version);
        }

        protected override bool IsClickOnce
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed;
            }
        }

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