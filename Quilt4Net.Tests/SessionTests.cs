using System.Reflection;
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
        public async Task x()
        {
            //Arrange
            var projectApiKey = "MyProjectApiKey";
            //var assembly = Assembly.GetExecutingAssembly();
            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            webApiClientMock.Setup(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>())).Returns(() => { return Task.Run(() => { }); });
            var configurationMock = new Mock<IConfiguration>(MockBehavior.Strict);
            configurationMock.SetupGet(x => x.ProjectApiKey).Returns(projectApiKey);
            configurationMock.SetupGet(x => x.Session.Environment).Returns("MyEnvironment");
            var applicationHelperMock = new Mock<IApplicationHelper>(MockBehavior.Strict);
            applicationHelperMock.Setup(x => x.GetApplicationData()).Returns(new ApplicationData { Fingerprint = "A1" });
            var machineHelperMock = new Mock<IMachineHelper>(MockBehavior.Strict);
            machineHelperMock.Setup(x => x.GetMachineData()).Returns(new MachineData { Name = "B1" });
            var userHelperMock = new Mock<IUserHelper>(MockBehavior.Strict);
            userHelperMock.Setup(x => x.GetUser()).Returns(new UserData { Fingerprint = "C1", UserName = "C2"});
            var session = new Session(webApiClientMock.Object, configurationMock.Object, applicationHelperMock.Object, machineHelperMock.Object, userHelperMock.Object);

            //Act
            await session.RegisterAsync();

            //Assert
            webApiClientMock.Verify(x => x.CreateAsync(It.IsAny<string>(), It.IsAny<SessionData>()), Times.Once);
        }
    }
}