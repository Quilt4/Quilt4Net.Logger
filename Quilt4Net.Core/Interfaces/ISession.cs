using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISession
    {
        event EventHandler<SessionRegistrationStartedEventArgs> SessionRegistrationStartedEvent;
        event EventHandler<SessionRegistrationCompletedEventArgs> SessionRegistrationCompletedEvent;
        event EventHandler<SessionEndStartedEventArgs> SessionEndStartedEvent;
        event EventHandler<SessionEndCompletedEventArgs> SessionEndCompletedEvent;
        Task<SessionResult> RegisterAsync();
        Task<SessionResult> RegisterAsync(Assembly firstAssembly);
        void RegisterStart();
        void RegisterStart(Assembly firstAssembly);
        SessionResult Register();
        SessionResult Register(Assembly firstAssembly);
        Task EndAsync();
        void End();
        bool IsRegistered { get; }
        Task<Guid> GetSessionKey();
        Task<IEnumerable<SessionRequest>> GetListAsync();
    }
}