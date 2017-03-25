using System;

namespace Quilt4Net.Core.Interfaces
{
    public interface ITargetConfiguration
    {
        string Location { get; set; }
        TimeSpan Timeout { get; set; }
    }
}