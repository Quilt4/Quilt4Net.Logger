using System;
using System.Runtime.Remoting.Channels;
using Moq;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Tests
{
    public static class Register_session_setup
    {
        internal  static Session GivenThereIsASession(Mock<IWebApiClient> webApiClientMock, Mock<IConfiguration> configurationMock, Action<SessionRegisterStartedEventArgs> sessionStartedAction, Action<SessionRegisterCompletedEventArgs> sessionCompletedAction)
        {
            var applicationHelperMock = new Mock<IApplicationHelper>(MockBehavior.Strict);
            applicationHelperMock.Setup(x => x.GetApplicationData()).Returns(() => new ApplicationData());
            var machineHelperMock = new Mock<IMachineHelper>(MockBehavior.Strict);
            machineHelperMock.Setup(x => x.GetMachineData()).Returns(() => new MachineData());
            var userHelperMock = new Mock<IUserHelper>(MockBehavior.Strict);
            userHelperMock.Setup(x => x.GetUser()).Returns(() => new UserData());
            var session = new Session(webApiClientMock.Object, configurationMock.Object, applicationHelperMock.Object, machineHelperMock.Object, userHelperMock.Object);
            session.SessionRegisteredStartedEvent += (semder, e) => { sessionStartedAction?.Invoke(e); };
            session.SessionRegisteredCompletedEvent += (sender, e) => { sessionCompletedAction?.Invoke(e); };
            return session;
        }
    }
}