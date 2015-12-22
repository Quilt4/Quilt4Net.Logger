using System;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.DataTransfer
{
    public class SessionResponse : ISessionData
    {
        public Guid SessionKey { get; set; }
        public DateTime ServerStartTime { get; set; }
        public DateTime ClientStartTime { get; set; }
        public DateTime? ClientEndTime { get; set; }
        public string Environment { get; set; }
        public ApplicationData Application { get; set; }
        public MachineData Machine { get; set; }
        public UserData User { get; set; }
    }

    public class SessionRequest : ISessionData
    {
        internal SessionRequest()
        {
        }

        public string ProjectApiKey { get; internal set; }
        public Guid SessionKey { get; internal set; }
        public DateTime ClientStartTime { get; internal set; }
        public string Environment { get; internal set; }
        public ApplicationData Application { get; internal set; }
        public MachineData Machine { get; internal set; }
        public UserData User { get; internal set; }
    }
}