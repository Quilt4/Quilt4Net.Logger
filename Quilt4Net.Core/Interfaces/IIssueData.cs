using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface IIssueData
    {
        Guid IssueKey { get; }
        Guid SessionKey { get; }
        DateTime ClientTime { get; }
        IDictionary<string, string> Data { get; }
        IssueTypeData IssueType { get; }
        Guid? IssueThreadKey { get; }
        string UserHandle { get; }
    }
}