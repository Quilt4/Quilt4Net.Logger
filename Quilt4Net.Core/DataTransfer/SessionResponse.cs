using System;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.DataTransfer
{
    public class SessionResponse //: ISessionData
    {
        public string SessionToken { get; set; }
        public DateTime ServerStartTime { get; set; }
        //public DateTime ClientStartTime { get; set; }
        //public string Environment { get; set; }
        //public ApplicationData Application { get; set; }
        //public MachineData Machine { get; set; }
        //public UserData User { get; set; }
        public string SessionUrl { get; set; }
    }
}