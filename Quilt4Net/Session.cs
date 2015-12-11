using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net
{
    public class Session : Core.Session
    {
        public Session(IWebApiClient webApiClient, IApplicationHelper applicationHelper, IMachineHelper machineHelper, IUserHelper userHelper)
            : base(webApiClient, applicationHelper, machineHelper, userHelper)
        {
        }

        public override async Task RegisterAsync(string projectApiKey, string environment)
        {
            await base.RegisterAsync(projectApiKey, environment, Assembly.GetEntryAssembly());
        }
    }
}