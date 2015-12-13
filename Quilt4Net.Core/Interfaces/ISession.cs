using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISession
    {
        event EventHandler<SessionRegisteredEventArgs> SessionRegisteredEvent;
        bool IsRegistered { get; }
        Task RegisterAsync();
        void RegisterStart();
        SessionResponse Register();
        Task<IEnumerable<SessionData>> GetListAsync();
    }
}