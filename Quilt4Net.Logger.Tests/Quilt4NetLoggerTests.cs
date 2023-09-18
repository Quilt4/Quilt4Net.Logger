using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Quilt4Net.Internals;
using Xunit;

namespace Quilt4Net.Logger.Tests;

public class Quilt4NetLoggerTests
{
    [Fact]
    public void Basic()
    {
        //Arrange
        var sender = new Mock<ISender>(MockBehavior.Strict);
        sender.Setup(x => x.Send(It.IsAny<LogInput>()));
        var configurationDataLoader = new Mock<IConfigurationDataLoader>(MockBehavior.Strict);
        configurationDataLoader.Setup(x => x.Get()).Returns(Mock.Of<ConfigurationData>());
        var categoryName = new Fixture().Create<string>();
        var sut = new Quilt4NetLogger(sender.Object, configurationDataLoader.Object, categoryName);

        var state = new Dictionary<string, object> { { "Some", "Data" } };

        //Act
        sut.Log(LogLevel.Information, new EventId(), state, null, (s, _) =>
        {
            return "Yeee!";
        });

        //Assert
        sender.Verify(x => x.Send(It.IsAny<LogInput>()), Times.Once);
    }
}