using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface IMachineHelper
    {
        MachineData GetMachineData();
        MachineData GetMachineData(string machineName);
    }
}