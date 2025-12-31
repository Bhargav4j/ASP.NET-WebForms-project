using Xunit;
using Films.Web.Pages;
using Microsoft.Extensions.Logging;
using Moq;

namespace Films.Web.Pages.Tests;

public class IndexModelTests
{
    private readonly Mock<ILogger<IndexModel>> _mockLogger;
    private readonly IndexModel _model;

    public IndexModelTests()
    {
        _mockLogger = new Mock<ILogger<IndexModel>>();
        _model = new IndexModel(_mockLogger.Object);
    }

    [Fact]
    public void IndexModel_Constructor_InitializesSuccessfully()
    {
        // Arrange
        var logger = new Mock<ILogger<IndexModel>>();

        // Act
        var model = new IndexModel(logger.Object);

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void OnGet_ExecutesSuccessfully()
    {
        // Act
        _model.OnGet();

        // Assert - No exception thrown
        Assert.NotNull(_model);
    }

    [Fact]
    public void OnGet_LogsInformation()
    {
        // Act
        _model.OnGet();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public void IndexModel_Constructor_AcceptsLogger()
    {
        // Arrange
        var logger = new Mock<ILogger<IndexModel>>();

        // Act
        var model = new IndexModel(logger.Object);

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void OnGet_CanBeCalledMultipleTimes()
    {
        // Act
        _model.OnGet();
        _model.OnGet();
        _model.OnGet();

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Exactly(3));
    }
}
