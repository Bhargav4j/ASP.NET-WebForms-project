using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using Films.Web.Pages;

namespace Films.Tests.Web.Pages;

public class IndexModelTests
{
    [Fact]
    public void IndexModel_Constructor_ShouldCreateInstance()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<IndexModel>>();

        // Act
        var model = new IndexModel(loggerMock.Object);

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void IndexModel_OnGet_ShouldExecuteWithoutException()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<IndexModel>>();
        var model = new IndexModel(loggerMock.Object);

        // Act
        model.OnGet();

        // Assert
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Home page accessed")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}
