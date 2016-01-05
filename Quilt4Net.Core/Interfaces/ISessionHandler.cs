using System;
using System.Reflection;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISessionHandler
    {
        event EventHandler<SessionRegistrationStartedEventArgs> SessionRegistrationStartedEvent;
        event EventHandler<SessionRegistrationCompletedEventArgs> SessionRegistrationCompletedEvent;
        event EventHandler<SessionEndStartedEventArgs> SessionEndStartedEvent;
        event EventHandler<SessionEndCompletedEventArgs> SessionEndCompletedEvent;
        bool IsRegistered { get; }
        DateTime ClientStartTime { get; }
        Task<SessionResult> RegisterAsync();
        Task<SessionResult> RegisterAsync(Assembly firstAssembly);
        void RegisterStart();
        void RegisterStart(Assembly firstAssembly);
        SessionResult Register();
        SessionResult Register(Assembly firstAssembly);
        Task EndAsync();
        void End();
        Task<string> GetSessionTokenAsync();
        //Task<IEnumerable<SessionResponse>> GetListAsync();
    }
}