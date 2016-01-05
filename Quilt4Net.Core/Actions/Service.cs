using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Core.Actions
{
    public class Service : IService
    {
        internal Service(IWebApiClient webApiClient)
        {
            Log = new Log(webApiClient);
        }

        public ILog Log { get; }
    }
}