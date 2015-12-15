using System;

namespace Quilt4Net.Core.Interfaces
{
    public interface ITargetConfiguration
    {
        string Location { get; }
        TimeSpan Timeout { get; }
    }
}