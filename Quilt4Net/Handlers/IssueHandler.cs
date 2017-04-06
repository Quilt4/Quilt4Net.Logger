using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class IssueHandler : IssueHandlerBase, Interfaces.IIssueHandler
    {
        public IssueHandler(ISessionHandler sessionHandler)
            : base(sessionHandler)
        {
        }
    }
}