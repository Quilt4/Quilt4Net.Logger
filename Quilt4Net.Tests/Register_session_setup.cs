using System;
using Moq;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Events;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Tests
{
    public static class Register_session_setup
    {
        internal static Session GivenThereIsASession(Mock<IWebApiClient> webApiClientMock, Mock<IConfiguration> configurationMock, Action<SessionRegistrationStartedEventArgs> sessionStartedAction, Action<SessionRegistrationCompletedEventArgs> sessionCompletedAction)
        {
            var applicationHelperMock = new Mock<IApplicationHelper>(MockBehavior.Strict);
            applicationHelperMock.Setup(x => x.GetApplicationData()).Returns(() => new ApplicationData());
            var machineHelperMock = new Mock<IMachineHelper>(MockBehavior.Strict);
            machineHelperMock.Setup(x => x.GetMachineData()).Returns(() => new MachineData());
            var userHelperMock = new Mock<IUserHelper>(MockBehavior.Strict);
            userHelperMock.Setup(x => x.GetDataUser()).Returns(() => new UserData());
            var session = new Session(webApiClientMock.Object as IWebApiClient, configurationMock.Object as IConfiguration, applicationHelperMock.Object as IApplicationHelper, machineHelperMock.Object as IMachineHelper, userHelperMock.Object as IUserHelper);
            session.SessionRegistrationStartedEvent += (semder, e) => { sessionStartedAction?.Invoke(e); };
            session.SessionRegistrationCompletedEvent += (sender, e) => { sessionCompletedAction?.Invoke(e); };
            return session;
        }
    }
}