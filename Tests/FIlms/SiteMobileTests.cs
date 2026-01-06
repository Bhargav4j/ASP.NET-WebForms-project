using System;
using Xunit;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace FIlms.Tests
{
    public class SiteMobileTests
    {
        [Fact]
        public void Site_Mobile_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var siteMobile = new Site_Mobile();

            // Assert
            Assert.NotNull(siteMobile);
        }

        [Fact]
        public void Site_Mobile_ShouldInheritFromMasterPage()
        {
            // Arrange
            var siteMobile = new Site_Mobile();

            // Act & Assert
            Assert.IsAssignableFrom<MasterPage>(siteMobile);
        }

        [Fact]
        public void Site_Mobile_TypeShouldBePublic()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act & Assert
            Assert.True(siteMobileType.IsPublic);
        }

        [Fact]
        public void Site_Mobile_ShouldBePartialClass()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act & Assert
            Assert.True(siteMobileType.IsClass);
        }

        [Fact]
        public void Site_Mobile_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act & Assert
            Assert.Equal("FIlms", siteMobileType.Namespace);
        }

        [Fact]
        public void Page_Load_MethodExists()
        {
            // Arrange
            var siteMobile = new Site_Mobile();
            var method = siteMobile.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Page_Load_HasCorrectSignature()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);
            var method = siteMobileType.GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Page_Load_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var siteMobile = new Site_Mobile();

            // Act & Assert
            var exception = Record.Exception(() => siteMobile.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(siteMobile, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_WithValidEventArgs_ShouldNotThrow()
        {
            // Arrange
            var siteMobile = new Site_Mobile();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => siteMobile.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(siteMobile, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }
    }
}
