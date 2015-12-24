using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Quilt4Net.Core;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Tests
{
    [TestFixture]
    public class Register_session_using_synchronous_method
    {
        [Test]
        public void When_registering_session_with_no_projectApiKey_set()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Default);

            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync<SessionRequest, SessionResponse>(It.IsAny<string>(), It.IsAny<SessionRequest>())).Returns(Task.FromResult(default(SessionResponse)));

            var session = Register_session_setup.GivenThereIsASession(webApiClientMock, configurationMock, null, null);

            ExpectedIssues.ProjectApiKeyNotSetException exception = null;
            SessionResult result = null;

            //Act
            try
            {
                result = session.Register();
            }
            catch (ExpectedIssues.ProjectApiKeyNotSetException exp)
            {
                exception = exp;
            }

            //Assert
            Assert.That(exception, Is.Not.Null);
            Assert.That(result, Is.Null);
            webApiClientMock.Verify(x => x.CreateAsync<SessionRequest, SessionResponse>(It.IsAny<string>(), It.IsAny<SessionRequest>()), Times.Never);
        }

        [Test]
        public void When_registering_session()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Default);
            configurationMock.SetupGet(x => x.ProjectApiKey).Returns("ABC123");

            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync<SessionRequest, SessionResponse>(It.IsAny<string>(), It.IsAny<SessionRequest>())).Returns(Task.FromResult(default(SessionResponse)));

            var session = Register_session_setup.GivenThereIsASession(webApiClientMock, configurationMock, null, null);

            //Act
            var response = session.Register();

            //Assert
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.EqualTo(true));
            Assert.That(response.ErrorMessage, Is.Null);
            Assert.That(response.Elapsed.Ticks, Is.GreaterThan(1));
            webApiClientMock.Verify(x => x.CreateAsync<SessionRequest, SessionResponse>(It.IsAny<string>(), It.IsAny<SessionRequest>()), Times.Once);
        }
    }
}