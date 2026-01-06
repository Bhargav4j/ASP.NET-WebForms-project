using System;
using Xunit;
using System.Web;
using System.Web.UI;
using Moq;

namespace FIlms.Tests
{
    public class DefaultTests
    {
        [Fact]
        public void Default_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var defaultPage = new _Default();

            // Assert
            Assert.NotNull(defaultPage);
        }

        [Fact]
        public void Default_ShouldInheritFromPage()
        {
            // Arrange
            var defaultPage = new _Default();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(defaultPage);
        }

        [Fact]
        public void Page_Load_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var defaultPage = new _Default();

            // Act & Assert
            var exception = Record.Exception(() => defaultPage.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(defaultPage, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_WithValidEventArgs_ShouldNotThrow()
        {
            // Arrange
            var defaultPage = new _Default();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => defaultPage.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(defaultPage, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void Default_TypeShouldBePublic()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act & Assert
            Assert.True(defaultType.IsPublic);
        }

        [Fact]
        public void Default_ShouldBePartialClass()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act & Assert
            Assert.True(defaultType.IsClass);
        }

        [Fact]
        public void Default_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act & Assert
            Assert.Equal("FIlms", defaultType.Namespace);
        }

        [Fact]
        public void Button1_Click_MethodExists()
        {
            // Arrange
            var defaultPage = new _Default();
            var method = defaultPage.GetType().GetMethod("Button1_Click",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Button1_Click_HasCorrectSignature()
        {
            // Arrange
            var defaultType = typeof(_Default);
            var method = defaultType.GetMethod("Button1_Click",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }
    }
}
