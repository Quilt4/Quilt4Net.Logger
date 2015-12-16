using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Quilt4Net.Core.DataTransfer;
using Quilt4Net.Core.Interfaces;

namespace Quilt4Net.Tests
{
    [TestFixture]
    public class SessionTests
    {
        [Test]
        public async Task When_registering_session()
        {
            //Arrange
            Exception exception = null;
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Strict);
            configurationMock.SetupGet(x => x.Target.Location).Returns("https://www.quilt4.com/");
            configurationMock.SetupGet(x => x.Target.Timeout).Returns(new TimeSpan(0, 0, 0, 1));
            configurationMock.SetupGet(x => x.ProjectApiKey).Returns("ABC123");
            configurationMock.SetupGet(x => x.Session.Environment).Returns("Test");
            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>())).Returns(Task.FromResult(default(SessionData)));
            var applicationHelperMock = new Mock<IApplicationHelper>(MockBehavior.Strict);
            applicationHelperMock.Setup(x => x.GetApplicationData()).Returns(() => new ApplicationData());
            var machineHelperMock = new Mock<IMachineHelper>(MockBehavior.Strict);
            machineHelperMock.Setup(x => x.GetMachineData()).Returns(() => new MachineData());
            var userHelperMock = new Mock<IUserHelper>(MockBehavior.Strict);
            userHelperMock.Setup(x => x.GetUser()).Returns(() => new UserData());
            var session = new Session(webApiClientMock.Object, configurationMock.Object, applicationHelperMock.Object, machineHelperMock.Object, userHelperMock.Object);
            SessionResponse response = null;

            //Act
            try
            {
                response = await session.RegisterAsync();                
            }
            catch (Exception exp)
            {
                exception = exp;
            }

            //Assert
            Assert.That(exception, Is.Null);
            Assert.That(response, Is.Not.Null);
            Assert.That(response.IsSuccess, Is.EqualTo(true));
            webApiClientMock.Verify(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>()), Times.Once);
        }
    }
}