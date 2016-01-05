using System.Collections.Generic;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core
{
    internal abstract class MachineInformationBase : IMachineInformation
    {
        private readonly IHashHandler _hashHandler;

        protected abstract string GetMachineName();
        protected abstract string GetCpuId();
        protected abstract string GetDriveSerial();
        protected abstract string GetOsName();
        protected abstract string GetModel();
        protected abstract string GetScreen();
        protected abstract string GetTimeZone();
        protected abstract string GetLanguage();

        protected MachineInformationBase(IHashHandler hashHandler)
        {
            _hashHandler = hashHandler;
        }

        public MachineData GetMachineData()
        {
            var machineName = GetMachineName();
            var fingerprint = $"MI1:{_hashHandler.ToMd5Hash($"{GetCpuId()}{GetDriveSerial()}{machineName}")}";
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