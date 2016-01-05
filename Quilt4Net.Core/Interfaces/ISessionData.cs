using System;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISessionData
    {
        DateTime ClientStartTime { get; }
        string Environment { get; }
        ApplicationData Application { get; }
        MachineData Machine { get; }
        UserData User { get; }
    }
}