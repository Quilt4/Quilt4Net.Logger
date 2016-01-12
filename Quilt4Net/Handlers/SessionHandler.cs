using Quilt4Net.Core;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class SessionHandler : SessionHandlerBase
    {
        public SessionHandler(IQuilt4NetClient client)
            : base(client)
        {
        }
    }
}