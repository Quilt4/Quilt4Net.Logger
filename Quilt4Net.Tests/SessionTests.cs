using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Quilt4Net.Core;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Tests
{
    [TestFixture]
    public class SessionTests
    {
        [Test]
        public async Task When_registering_session_with_no_projectApiKey_set()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Default);

            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>())).Returns(Task.FromResult(default(SessionData)));

            var session = GivenThereIsASession(webApiClientMock, configurationMock);

            ExpectedIssues.ProjectApiKeyNotSetException exception = null;
            SessionResponse response = null;

            //Act
            try
            {
                response = await session.RegisterAsync();
            }
            catch (ExpectedIssues.ProjectApiKeyNotSetException exp)
            {
                exception = exp;
            }

            //Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(response, Is.Null);
            webApiClientMock.Verify(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>()), Times.Never);
        }

        [Test]
        public async Task When_registering_session()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Default);
            configurationMock.SetupGet(x => x.ProjectApiKey).Returns("ABC123");

            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>())).Returns(Task.FromResult(default(SessionData)));

            var session = GivenThereIsASession(webApiClientMock, configurationMock);

            //Act
            var response = await session.RegisterAsync();

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.EqualTo(true));
            Assert.That(response.ErrorMessage, Is.Null);
            Assert.That(response.Elapsed.Ticks, Is.GreaterThan(1));
            webApiClientMock.Verify(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>()), Times.Once);
        }

        private static Session GivenThereIsASession(Mock<IWebApiClient> webApiClientMock, Mock<IConfiguration> configurationMock)
        {
            var applicationHelperMock = new Mock<IApplicationHelper>(MockBehavior.Strict);
            applicationHelperMock.Setup(x => x.GetApplicationData()).Returns(() => new ApplicationData());
            var machineHelperMock = new Mock<IMachineHelper>(MockBehavior.Strict);
            machineHelperMock.Setup(x => x.GetMachineData()).Returns(() => new MachineData());
            var userHelperMock = new Mock<IUserHelper>(MockBehavior.Strict);
            userHelperMock.Setup(x => x.GetUser()).Returns(() => new UserData());
            var session = new Session(webApiClientMock.Object, configurationMock.Object, applicationHelperMock.Object, machineHelperMock.Object, userHelperMock.Object);
            return session;
        }
    }
}