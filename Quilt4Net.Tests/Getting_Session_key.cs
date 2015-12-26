using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Tests
{
    [TestFixture]
    public class Getting_Session_key
    {
        [Test]
        public async void When_several_threads_are_getting_session_key_at_the_same_time()
        {
            //Arrange
            var sessionRegistrationStartedEventCount = 0;
            var sessionRegistrationCompletedEventCount = 0;
            var sessionKey = Guid.NewGuid();
            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync<SessionRequest, SessionResponse>(It.IsAny<string>(), It.IsAny<SessionRequest>())).Returns(Task.FromResult(new SessionResponse { SessionKey = sessionKey })).Callback(() => { System.Threading.Thread.Sleep(500); });
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Strict);
            configurationMock.SetupGet(x => x.ProjectApiKey).Returns("ABC123");
            configurationMock.SetupGet(x => x.Session.Environment).Returns((string)null);
            var applicationHelperMock = new Mock<IApplicationHelper>(MockBehavior.Strict);
            applicationHelperMock.Setup(x => x.GetApplicationData()).Returns(new ApplicationData {});
            var machineHelperMock = new Mock<IMachineHelper>(MockBehavior.Strict);
            machineHelperMock.Setup(x => x.GetMachineData()).Returns(new MachineData { });
            var userHelperMock = new Mock<IUserHelper>(MockBehavior.Strict);
            userHelperMock.Setup(x => x.GetDataUser()).Returns(new UserData { });
            var session = new Session(webApiClientMock.Object, configurationMock.Object, applicationHelperMock.Object, machineHelperMock.Object, userHelperMock.Object);
            session.SessionRegistrationStartedEvent += delegate { sessionRegistrationStartedEventCount++; };
            session.SessionRegistrationCompletedEvent += delegate { sessionRegistrationCompletedEventCount++; };

            //Act
            var task1 = Task.Run(() => session.GetSessionKeyAsync());
            var task2 = Task.Run(() => session.GetSessionKeyAsync());
            var task3 = Task.Run(() => { System.Threading.Thread.Sleep(100); return session.GetSessionKeyAsync(); });
            Task.WaitAll(task1, task2, task3);

            //Assert
            Assert.That(task1.Result, Is.Not.EqualTo(Guid.Empty));
            Assert.That(task1.Result, Is.EqualTo(task2.Result));
            Assert.That(task1.Result, Is.EqualTo(task3.Result));
            Assert.That(sessionRegistrationStartedEventCount, Is.EqualTo(1));
            Assert.That(sessionRegistrationCompletedEventCount, Is.EqualTo(1));
            webApiClientMock.Verify(x => x.CreateAsync<SessionRequest, SessionResponse>(It.IsAny<string>(), It.IsAny<SessionRequest>()), Times.Once);
        }

        [Test]
        public async void When_several_threads_are_ending_session_at_the_same_time()
        {
            //Arrange
            var sessionEndCompletedEventCount = 0;
            var sessionEndStartedEventCount = 0;
            var sessionKey = Guid.NewGuid();
            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync<SessionRequest, SessionResponse>(It.IsAny<string>(), It.IsAny<SessionRequest>())).Returns(Task.FromResult(new SessionResponse { SessionKey = sessionKey }));
            webApiClientMock.Setup(x => x.ExecuteCommandAsync<Guid>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>())).Returns(Task.FromResult<object>(null)).Callback(() => { System.Threading.Thread.Sleep(500); });
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Strict);
            configurationMock.SetupGet(x => x.ProjectApiKey).Returns("ABC123");
            configurationMock.SetupGet(x => x.Session.Environment).Returns((string)null);
            var applicationHelperMock = new Mock<IApplicationHelper>(MockBehavior.Strict);
            applicationHelperMock.Setup(x => x.GetApplicationData()).Returns(new ApplicationData { });
            var machineHelperMock = new Mock<IMachineHelper>(MockBehavior.Strict);
            machineHelperMock.Setup(x => x.GetMachineData()).Returns(new MachineData { });
            var userHelperMock = new Mock<IUserHelper>(MockBehavior.Strict);
            userHelperMock.Setup(x => x.GetDataUser()).Returns(new UserData { });
            var session = new Session(webApiClientMock.Object, configurationMock.Object, applicationHelperMock.Object, machineHelperMock.Object, userHelperMock.Object);
            session.SessionEndCompletedEvent += delegate { sessionEndCompletedEventCount++; };
            session.SessionEndStartedEvent += delegate { sessionEndStartedEventCount++; };
            var response = await session.GetSessionKeyAsync();

            //Act
            var task1 = Task.Run(() => session.EndAsync());
            var task2 = Task.Run(() => session.EndAsync());
            var task3 = Task.Run(() => { System.Threading.Thread.Sleep(1000); return session.EndAsync(); });
            Task.WaitAll(task1, task2, task3);

            //Assert
            Assert.That(session.IsRegistered, Is.False);
            Assert.That(sessionEndStartedEventCount, Is.EqualTo(1));
            Assert.That(sessionEndCompletedEventCount, Is.EqualTo(1));
            webApiClientMock.Verify(x => x.ExecuteCommandAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
        }
    }
}