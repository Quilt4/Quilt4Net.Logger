using System.Reflection;
using Quilt4Net.DataTransfer;

namespace Quilt4Net.Interfaces
{
    public interface IApplicationHelper
    {
        ApplicationData GetApplicationData(string projectApiKey, Assembly firstAssembly);
    }
}