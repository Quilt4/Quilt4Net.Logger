using System;
using System.IO;
using System.Management;
using System.Threading;

namespace Quilt4Net
{
    public class MachineHelper : Core.MachineHelper
    {
        protected override string GetMachineName()
        {
            return Environment.MachineName;
        }

        protected override string GetCpuId()
        {
            var cpuInfo = "Unknown";

            try
            {
                var mc = new ManagementClass("win32_processor");
                var moc = mc.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    if (string.IsNullOrEmpty(cpuInfo))
                    {
                        cpuInfo = mo.Properties["processorID"].Value.ToString();
                        break;
                    }
                }
            }
            catch
            {
                cpuInfo = "N/A";
            }

            return cpuInfo;
        }

        protected override string GetDriveSerial()
        {
            var driveSerial = string.Empty;

            var drives = Directory.GetLogicalDrives();
            foreach (var drive in drives)
            {
                if (!drive.StartsWith("A") && !drive.StartsWith("B"))
                {
                    driveSerial = GetHarddriveId(drive[0]);
                    if (!string.IsNullOrEmpty(driveSerial)) break;
                }
            }

            return driveSerial;
        }

        private static string GetHarddriveId(char drive)
        {
            var dsk = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}:\"", drive));
            dsk.Get();
            return dsk["VolumeSerialNumber"].ToString();
        }

        protected override string GetOsName()
        {
            var response = "Unknown";

            try
            {
                const string query = "SELECT * FROM Win32_OperatingSystem";
                using (var searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject info in searcher.Get())
                    {
                        try
                        {
                            response = info["Caption"].ToString();
                            break;
                        }
                        catch (Exception exception)
                        {
                            System.Diagnostics.Debug.WriteLine(exception.Message);
                            response = "N/A";
                        }
                    }
                }
            }
            catch
            {
                try
                {
                    response = Environment.OSVersion.ToString();
                }
                catch
                {
                    response = "N/A";
                }
            }

            return response;
        }

        protected override string GetModel()
        {
            var model = string.Empty;

            var mc = new ManagementClass("win32_processor");
            var moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (string.IsNullOrEmpty(model))
                {
                    model = mo.Properties["Name"].Value.ToString();
                    break;
                }
            }

            return model;
        }

        protected override string GetScreen()
        {
            var response = "Unknown";

            try
            {
                const string GraphicsQuery = "SELECT * FROM Win32_VideoController";
                using (var graphics_searcher = new ManagementObjectSearcher(GraphicsQuery))
                {
                    foreach (ManagementObject info in graphics_searcher.Get())
                    {
                        try
                        {
                            //response = info["Name"] + " (" + info["CurrentHorizontalResolution"] + " x " + info["CurrentVerticalResolution"] + ")";
                            response = string.Format("{0}x{1}", info["CurrentHorizontalResolution"], info["CurrentVerticalResolution"]);
                            break;
                        }
                        catch (Exception exception)
                        {
                            System.Diagnostics.Debug.WriteLine(exception.Message);
                            response = "N/A";
                        }
                    }
                }
            }
            catch
            {
                response = "N/A";
            }

            return response;
        }

        protected override string GetTimeZone()
        {
            var response = "Unknown";

            try
            {
                var timeZone = TimeZoneInfo.Local;
                response = timeZone.Id;
            }
            catch
            {
                response = "N/A";
            }

            return response;
        }

        protected override string GetLanguage()
        {
            var response = "Unknown";

            try
            {
                var currentCulture = Thread.CurrentThread.CurrentCulture;
                response = currentCulture.Name;
            }
            catch
            {
                response = "N/A";
            }

            return response;
        }
    }
}