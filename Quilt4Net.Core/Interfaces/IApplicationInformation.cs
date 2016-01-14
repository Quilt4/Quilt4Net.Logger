using System.Reflection;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface IApplicationInformation
    {
        string Version { get; }
        ApplicationData GetApplicationData();
        void SetFirstAssembly(Assembly firstAssembly);
    }
}