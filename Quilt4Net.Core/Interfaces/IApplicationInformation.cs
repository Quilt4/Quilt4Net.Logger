using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface IApplicationInformation
    {
        ApplicationData GetApplicationData();
        void SetApplicationNameVersion(ApplicationNameVersion applicationNameVersion);
        ApplicationNameVersion GetApplicationNameVersion();
    }
}