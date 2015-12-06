using System;
using System.Collections.Generic;
using Quilt4Net.DataTransfer;
using Quilt4Net.Interfaces;

namespace Quilt4Net
{
    internal class MachineHelper : IMachineHelper
    {
        public MachineData GetMachineData()
        {
            var fingerprint = $"MI1:{$"{GetCpuId()}{GetDriveSerial()}{GetMachineName()}".ToMd5Hash()}";
            var data = new Dictionary<string, string> { { "OsName", GetOsName() }, { "Model", GetModel() }, { "Type", "Desktop" }, { "Screen", GetScreen() }, { "TimeZone", GetTimeZone() }, { "Language", GetLanguage() } };

            var machine = new MachineData
            {
                Name = GetMachineName(),
                Fingerprint = fingerprint,
                Data = data
            };
            return machine;
        }

        private static string GetCpuId()
        {
            throw new NotImplementedException();
            //var cpuInfo = "Unknown";
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

            //return cpuInfo;
        }

        private static string GetDriveSerial()
        {
            throw new NotImplementedException();
            //var driveSerial = string.Empty;

            //var drives = Directory.GetLogicalDrives();
            //foreach (var drive in drives)
            //{
            //    if (!drive.StartsWith("A") && !drive.StartsWith("B"))
            //    {
            //        driveSerial = GetHarddriveId(drive[0]);
            //        if (!string.IsNullOrEmpty(driveSerial)) break;
            //    }
            //}

            //return driveSerial;
        }

        internal static string GetMachineName()
        {
            throw new NotImplementedException();
            //return Environment.MachineName;
        }

        internal static string GetOsName()
        {
            throw new NotImplementedException();
            //var response = "Unknown";
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

            //return response;
        }

        internal static string GetModel()
        {
            throw new NotImplementedException();
            //var model = string.Empty;
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

            //return model;
        }

        internal static string GetScreen()
        {
            throw new NotImplementedException();
            //var response = "Unknown";
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

            //return response;
        }

        public static string GetTimeZone()
        {
            throw new NotImplementedException();
            //try
            //{
            //    var timeZone = TimeZoneInfo.Local;
            //    return timeZone.Id;
            //}
            //catch
            //{
            //    return "N/A";
            //}
        }

        public static string GetLanguage()
        {
            throw new NotImplementedException();
            //try
            //{
            //    var currentCulture = Thread.CurrentThread.CurrentCulture;
            //    return currentCulture.Name;
            //}
            //catch
            //{
            //    return "N/A";
            //}
        }
    }
}