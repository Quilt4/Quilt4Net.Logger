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
            //var applicationHelperMock = new Mock<IApplicationInformation>(MockBehavior.Strict);
            //applicationHelperMock.Setup(x => x.GetApplicationData()).Returns(() => new ApplicationData());
            //var machineHelperMock = new Mock<IMachineInformation>(MockBehavior.Strict);
            //machineHelperMock.Setup(x => x.GetMachineData()).Returns(() => new MachineData());
            //var userHelperMock = new Mock<IUserInformation>(MockBehavior.Strict);
            //userHelperMock.Setup(x => x.GetDataUser()).Returns(() => new UserData());
            var clientMock = new Mock<IQuilt4NetClient>(MockBehavior.Strict);
            //clientMock.SetupGet(x => x.Configuration).Returns(() => configurationMock.Object);
            //clientMock.SetupGet(x => x.Client).Returns(() => webApiClientMock.Object);
            //clientMock.SetupGet(x => x.Information.Application).Returns(() => applicationHelperMock.Object);
            //clientMock.SetupGet(x => x.Information.User).Returns(() => userHelperMock.Object);
            //clientMock.SetupGet(x => x.Information.Machine).Returns(() => machineHelperMock.Object);
            var session = new SessionHandler(clientMock.Object);
            //session.SessionRegistrationStartedEvent += (semder, e) => { sessionStartedAction?.Invoke(e); };
            //session.SessionRegistrationCompletedEvent += (sender, e) => { sessionCompletedAction?.Invoke(e); };
            return session;
        }
    }
}