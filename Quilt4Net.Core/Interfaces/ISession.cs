using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISession
    {
        event EventHandler<SessionRegistrationStartedEventArgs> SessionRegistrationStartedEvent;
        event EventHandler<SessionRegistrationCompletedEventArgs> SessionRegistrationCompletedEvent;
        Task<SessionResult> RegisterAsync();
        void RegisterStart();
        SessionResult Register();
        Task EndAsync();
        void End();
        bool IsRegistered { get; }
        Task<Guid> GetSessionKey();
        Task<IEnumerable<SessionRequest>> GetListAsync();
    }
}