using System;
using System.Collections.Generic;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    public abstract class MachineHelper : IMachineHelper
    {
        public abstract MachineData GetMachineData();

        internal abstract string GetMachineName();

        public MachineData GetMachineData(string machineName)
        {
            var fingerprint = $"MI1:{$"{GetCpuId()}{GetDriveSerial()}{machineName}".ToMd5Hash()}";
            var data = new Dictionary<string, string> { { "OsName", GetOsName() }, { "Model", GetModel() }, { "Type", "Desktop" }, { "Screen", GetScreen() }, { "TimeZone", GetTimeZone() }, { "Language", GetLanguage() } };

            var machine = new MachineData
            {
                Name = machineName,
                Fingerprint = fingerprint,
                Data = data
            };
            return machine;
        }

        private static string GetCpuId()
        {
            var cpuInfo = "Unknown";
            //TODO: Move this code to .NET override class
            //try
            //{
            //    var mc = new ManagementClass("win32_processor");
            //    var moc = mc.GetInstances();

            //    foreach (ManagementObject mo in moc)
            //    {
            //        if (string.IsNullOrEmpty(cpuInfo))
            //        {
            //            cpuInfo = mo.Properties["processorID"].Value.ToString();
            //            break;
            //        }
            //    }
            //}
            //catch
            //{
            //    cpuInfo = "N/A";
            //}

            return cpuInfo;
        }

        private static string GetDriveSerial()
        {
            var driveSerial = string.Empty;

            //TODO: Move this code to .NET override class
            //var drives = Directory.GetLogicalDrives();
            //foreach (var drive in drives)
            //{
            //    if (!drive.StartsWith("A") && !drive.StartsWith("B"))
            //    {
            //        driveSerial = GetHarddriveId(drive[0]);
            //        if (!string.IsNullOrEmpty(driveSerial)) break;
            //    }
            //}

            return driveSerial;
        }

        internal static string GetOsName()
        {
            var response = "Unknown";

            //TODO: Move this code to .NET override class
            //try
            //{
            //    const string query = "SELECT * FROM Win32_OperatingSystem";
            //    using (var searcher = new ManagementObjectSearcher(query))
            //    {
            //        foreach (ManagementObject info in searcher.Get())
            //        {
            //            try
            //            {
            //                response = info["Caption"].ToString();
            //                break;
            //            }
            //            catch (Exception exception)
            //            {
            //                System.Diagnostics.Debug.WriteLine(exception.Message);
            //                response = "N/A";
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //    try
            //    {
            //        response = Environment.OSVersion.ToString();
            //    }
            //    catch
            //    {
            //        response = "N/A";
            //    }
            //}

            return response;
        }

        internal static string GetModel()
        {
            var model = string.Empty;

            //TODO: Move this code to .NET override class
            //var mc = new ManagementClass("win32_processor");
            //var moc = mc.GetInstances();

            //foreach (ManagementObject mo in moc)
            //{
            //    if (string.IsNullOrEmpty(model))
            //    {
            //        model = mo.Properties["Name"].Value.ToString();
            //        break;
            //    }
            //}

            return model;
        }

        internal static string GetScreen()
        {
            var response = "Unknown";

            //TODO: Move this code to .NET override class
            //try
            //{
            //    const string GraphicsQuery = "SELECT * FROM Win32_VideoController";
            //    using (var graphics_searcher = new ManagementObjectSearcher(GraphicsQuery))
            //    {
            //        foreach (ManagementObject info in graphics_searcher.Get())
            //        {
            //            try
            //            {
            //                //response = info["Name"] + " (" + info["CurrentHorizontalResolution"] + " x " + info["CurrentVerticalResolution"] + ")";
            //                response = string.Format("{0}x{1}", info["CurrentHorizontalResolution"], info["CurrentVerticalResolution"]);
            //                break;
            //            }
            //            catch (Exception exception)
            //            {
            //                System.Diagnostics.Debug.WriteLine(exception.Message);
            //                response = "N/A";
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //    response = "N/A";
            //}

            return response;
        }

        public static string GetTimeZone()
        {
            var response = "Unknown";

            //TODO: Move this code to .NET override class
            //try
            //{
            //    var timeZone = TimeZoneInfo.Local;
            //    response = timeZone.Id;
            //}
            //catch
            //{
            //    response = "N/A";
            //}

            return response;
        }

        public static string GetLanguage()
        {
            var response = "Unknown";

            //TODO: Move this code to .NET override class
            //try
            //{
            //    var currentCulture = Thread.CurrentThread.CurrentCulture;
            //    response = currentCulture.Name;
            //}
            //catch
            //{
            //    response = "N/A";
            //}

            return response;
        }
    }
}