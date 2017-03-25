using System.Collections.Generic;

namespace Quilt4Net.Core.DataTransfer
{
    public class MachineData
    {
        public string Fingerprint { get; set; }
        public string Name { get; set; }
        public IDictionary<string, string> Data { get; set; }
    }
}