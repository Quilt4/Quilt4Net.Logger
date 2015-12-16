using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISession
    {
        event EventHandler<SessionRegisterCompletedEventArgs> SessionRegisteredCompletedEvent;
        bool IsRegistered { get; }
        Task<SessionResponse> RegisterAsync();
        void RegisterStart();
        SessionResponse Register();
        Task<IEnumerable<SessionData>> GetListAsync();
    }
}