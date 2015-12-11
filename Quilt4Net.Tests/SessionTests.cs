using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Quilt4Net.Core;
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
            var webApiClientMock = new Mock<IWebApiClient>(MockBehavior.Strict);
            var applicationHelperMock = new Mock<IApplicationHelper>(MockBehavior.Strict);
            var machineHelperMock = new Mock<IMachineHelper>(MockBehavior.Strict);
            var userHelperMock = new Mock<IUserHelper>(MockBehavior.Strict);
            var session = new Session(webApiClientMock.Object, applicationHelperMock.Object, machineHelperMock.Object, userHelperMock.Object);

            //Act
            await session.RegisterAsync("MyProjectApiKey", "MyEnvironment");

            //Assert
            //Assert.That(response, Is.Not.Null);
        }
    }
}