using System.Collections.Generic;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal abstract class MachineHelper : IMachineHelper
    {
        protected abstract string GetMachineName();
        protected abstract string GetCpuId();
        protected abstract string GetDriveSerial();
        protected abstract string GetOsName();
        protected abstract string GetModel();
        protected abstract string GetScreen();
        protected abstract string GetTimeZone();
        protected abstract string GetLanguage();

        public MachineData GetMachineData()
        {
            var machineName = GetMachineName();
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
    }
}