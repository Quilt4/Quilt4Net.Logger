using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Core;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class SessionHandler : SessionHandlerBase, Interfaces.ISessionHandler
    {
        public SessionHandler(IQuilt4NetClient client)
            : base(client)
        {
        }

        public async Task<SessionResult> RegisterAsync(Assembly firstAssembly)
        {
            //Client.Information.Aplication.SetApplicationNameVersion(new ApplicationNameVersion(firstAssembly.GetName().Name, firstAssembly.GetName().Version.ToString()));
            (Client.Information.Aplication as ApplicationInformation)?.SetFirstAssembly(firstAssembly);
            return await RegisterAsync();
        }

        public void RegisterStart(Assembly firstAssembly)
        {
            //Client.Information.Aplication.SetApplicationNameVersion(new ApplicationNameVersion(firstAssembly.GetName().Name, firstAssembly.GetName().Version.ToString()));
            (Client.Information.Aplication as ApplicationInformation)?.SetFirstAssembly(firstAssembly);
            RegisterStart();
        }

        public SessionResult Register(Assembly firstAssembly)
        {
            //Client.Information.Aplication.SetApplicationNameVersion(new ApplicationNameVersion(firstAssembly.GetName().Name, firstAssembly.GetName().Version.ToString()));
            (Client.Information.Aplication as ApplicationInformation)?.SetFirstAssembly(firstAssembly);
            return Register();
        }
    }
}