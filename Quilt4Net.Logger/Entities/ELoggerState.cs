namespace Quilt4Net.Entities;

public enum ELoggerState
{
    Offline,
    Initiated,
    WaitingToStart,
    WaitingForConfiguration,
    Ready,
    Online,
    Crash,
}