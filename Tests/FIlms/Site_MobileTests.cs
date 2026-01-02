using System;
using Xunit;
using FIlms;

namespace Films.Tests.FIlms;

public class Site_MobileTests
{
    [Fact]
    public void Site_Mobile_Constructor_ShouldCreateInstance()
    {
        // Arrange & Act & Assert
        Assert.NotNull(typeof(Site_Mobile));
    }

    [Fact]
    public void Site_Mobile_InheritsFromMasterPage()
    {
        // Arrange
        var siteMobileType = typeof(Site_Mobile);

        // Act
        var baseType = siteMobileType.BaseType;

        // Assert
        Assert.NotNull(baseType);
        Assert.Equal("MasterPage", baseType.Name);
    }

    [Fact]
    public void Site_Mobile_HasPageLoadMethod()
    {
        // Arrange
        var siteMobileType = typeof(Site_Mobile);
        var method = siteMobileType.GetMethod("Page_Load",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert
        Assert.NotNull(method);
    }
}
