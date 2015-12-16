using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Quilt4Net.Core;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Tests
{
    [TestFixture]
    public class Register_session_using_start_method
    {
        [Test]
        public void When_registering_session_with_no_projectApiKey_set()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Default);

            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>())).Returns(Task.FromResult(default(SessionData)));

            var callbackEvent = new AutoResetEvent(false);

            var session = Register_session_setup.GivenThereIsASession(webApiClientMock, configurationMock, null, (e) => { callbackEvent.Set(); });

            ExpectedIssues.ProjectApiKeyNotSetException exception = null;
            SessionResponse response = null;

            //Act
            try
            {
                session.RegisterStart();
            }
            catch (ExpectedIssues.ProjectApiKeyNotSetException exp)
            {
                exception = exp;
            }

            //Assert
            if (callbackEvent.WaitOne(1000))
                throw new NotImplementedException("The SessionRegisteredEvent was invoked, it should not have been.");
        }

        [Test]
        public void When_registering_session()
        {
            //Arrange
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Default);
            configurationMock.SetupGet(x => x.ProjectApiKey).Returns("ABC123");

            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>())).Returns(Task.FromResult(default(SessionData)));

            var callbackEvent = new AutoResetEvent(false);

            var session = Register_session_setup.GivenThereIsASession(webApiClientMock, configurationMock, null, (e) =>
                {
                    Assert.That(e, Is.Not.Null);
                    Assert.That(e.Response.IsSuccess, Is.EqualTo(true));
                    Assert.That(e.Response.ErrorMessage, Is.Null);
                    Assert.That(e.Response.Elapsed.Ticks, Is.GreaterThan(1));
                    callbackEvent.Set();
                });

            //Act
            session.RegisterStart();

            //Assert
            if (!callbackEvent.WaitOne(1000))
                throw new NotImplementedException("The SessionRegisteredEvent was never invoked.");            
        }
    }
}