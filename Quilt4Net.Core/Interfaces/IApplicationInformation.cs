using System.Reflection;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface IApplicationInformation
    {
        ApplicationData GetApplicationData();
        void SetFirstAssembly(Assembly firstAssembly);
    }
}