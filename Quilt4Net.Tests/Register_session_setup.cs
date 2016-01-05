using System;
using Moq;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Tests
{
    public static class Register_session_setup
    {
        internal static SessionHandler GivenThereIsASession(Mock<IWebApiClient> webApiClientMock, Mock<IConfiguration> configurationMock, Action<SessionRegistrationStartedEventArgs> sessionStartedAction, Action<SessionRegistrationCompletedEventArgs> sessionCompletedAction)
        {
            var applicationHelperMock = new Mock<IApplicationLookup>(MockBehavior.Strict);
            applicationHelperMock.Setup(x => x.GetApplicationData()).Returns(() => new ApplicationData());
            var machineHelperMock = new Mock<IMachineLookup>(MockBehavior.Strict);
            machineHelperMock.Setup(x => x.GetMachineData()).Returns(() => new MachineData());
            var userHelperMock = new Mock<IUserLookup>(MockBehavior.Strict);
            userHelperMock.Setup(x => x.GetDataUser()).Returns(() => new UserData());
            var session = new SessionHandler(webApiClientMock.Object as IWebApiClient, configurationMock.Object as IConfiguration, applicationHelperMock.Object as IApplicationLookup, machineHelperMock.Object as IMachineLookup, userHelperMock.Object as IUserLookup);
            session.SessionRegistrationStartedEvent += (semder, e) => { sessionStartedAction?.Invoke(e); };
            session.SessionRegistrationCompletedEvent += (sender, e) => { sessionCompletedAction?.Invoke(e); };
            return session;
        }
    }
}