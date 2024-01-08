using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Quilt4Net.Dtos;
using Quilt4Net.Internals;
using Xunit;

namespace Quilt4Net.Logger.Tests;

public class Quilt4NetLoggerTests
{
    [Fact]
    public void Basic()
    {
        //Arrange
        var messageQueue = new Mock<IMessageQueue>(MockBehavior.Strict);
        messageQueue.Setup(x => x.Enqueue(It.IsAny<LogInput>()));
        var configurationDataLoader = new Mock<IConfigurationDataLoader>(MockBehavior.Strict);
        configurationDataLoader.Setup(x => x.Get()).Returns(Mock.Of<ConfigurationData>());
        var categoryName = new Fixture().Create<string>();
        var sut = new Quilt4NetLogger(messageQueue.Object, configurationDataLoader.Object, categoryName);

        var state = new Dictionary<string, object> { { "Some", "Data" } };

        //Act
        sut.Log(LogLevel.Information, new EventId(), state, null, (s, _) =>
        {
            return "Yeee!";
        });

        //Assert
        messageQueue.Verify(x => x.Enqueue(It.IsAny<LogInput>()), Times.Once);
    }
}