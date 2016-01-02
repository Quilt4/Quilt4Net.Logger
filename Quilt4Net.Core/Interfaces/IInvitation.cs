using System;
using System.Threading.Tasks;

namespace Quilt4Net.Core.Interfaces
{
    public interface IInvitation
    {
        Task CreateAsync(Guid projectKey, string user);
    }
}