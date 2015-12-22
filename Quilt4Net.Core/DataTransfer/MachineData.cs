using System.Collections.Generic;

namespace Quilt4Net.Core.DataTransfer
{
    public class MachineData
    {
        internal MachineData()
        {
        }

        public string Fingerprint { get; internal set; }
        public string Name { get; internal set; }
        public IDictionary<string, string> Data { get; internal set; }
    }
}