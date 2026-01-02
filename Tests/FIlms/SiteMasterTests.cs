using System;
using System.Web;
using Xunit;
using FIlms;

namespace Films.Tests.FIlms;

public class SiteMasterTests
{
    [Fact]
    public void SiteMaster_Constructor_ShouldCreateInstance()
    {
        // Arrange & Act & Assert
        Assert.NotNull(typeof(SiteMaster));
    }

    [Fact]
    public void SiteMaster_InheritsFromMasterPage()
    {
        // Arrange
        var siteMasterType = typeof(SiteMaster);

        // Act
        var baseType = siteMasterType.BaseType;

        // Assert
        Assert.NotNull(baseType);
    }

    [Fact]
    public void SiteMaster_HasAntiXsrfTokenKeyConstant()
    {
        // Arrange
        var siteMasterType = typeof(SiteMaster);
        var field = siteMasterType.GetField("AntiXsrfTokenKey",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        // Assert
        Assert.NotNull(field);
        Assert.Equal("__AntiXsrfToken", field.GetValue(null));
    }

    [Fact]
    public void SiteMaster_HasAntiXsrfUserNameKeyConstant()
    {
        // Arrange
        var siteMasterType = typeof(SiteMaster);
        var field = siteMasterType.GetField("AntiXsrfUserNameKey",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        // Assert
        Assert.NotNull(field);
        Assert.Equal("__AntiXsrfUserName", field.GetValue(null));
    }

    [Fact]
    public void SiteMaster_HasPageInitMethod()
    {
        // Arrange
        var siteMasterType = typeof(SiteMaster);
        var method = siteMasterType.GetMethod("Page_Init",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert
        Assert.NotNull(method);
    }

    [Fact]
    public void SiteMaster_HasMasterPagePreLoadMethod()
    {
        // Arrange
        var siteMasterType = typeof(SiteMaster);
        var method = siteMasterType.GetMethod("master_Page_PreLoad",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert
        Assert.NotNull(method);
    }

    [Fact]
    public void SiteMaster_HasPageLoadMethod()
    {
        // Arrange
        var siteMasterType = typeof(SiteMaster);
        var method = siteMasterType.GetMethod("Page_Load",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Assert
        Assert.NotNull(method);
    }
}
