using System;
using System.Threading.Tasks;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;

namespace Quilt4Net.Core.Interfaces
{
    public interface ISessionHandler : IDisposable
    {
        IQuilt4NetClient Client { get; }
        event EventHandler<SessionRegistrationStartedEventArgs> SessionRegistrationStartedEvent;
        event EventHandler<SessionRegistrationCompletedEventArgs> SessionRegistrationCompletedEvent;
        event EventHandler<SessionEndStartedEventArgs> SessionEndStartedEvent;
        event EventHandler<SessionEndCompletedEventArgs> SessionEndCompletedEvent;
        bool IsRegisteredOnServer { get; }
        string SessionUrl { get; }
        DateTime ClientStartTime { get; }
        string Environment { get; }
        IApplicationInformation Application { get; }
        Task<SessionResult> RegisterAsync();
        void RegisterStart();
        SessionResult Register();
        Task EndAsync();
        void End();
        Task<string> GetSessionKeyAsync();
        SessionRegistrationCompletedEventArgs LastSessionRegistrationCompletedEventArgs { get; }
    }
}