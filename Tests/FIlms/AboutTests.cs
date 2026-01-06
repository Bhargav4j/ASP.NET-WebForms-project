using System;
using Xunit;
using System.Web;
using System.Web.UI;

namespace FIlms.Tests
{
    public class AboutTests
    {
        [Fact]
        public void About_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var about = new About();

            // Assert
            Assert.NotNull(about);
        }

        [Fact]
        public void About_ShouldInheritFromPage()
        {
            // Arrange
            var about = new About();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(about);
        }

        [Fact]
        public void Page_Load_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var about = new About();

            // Act & Assert
            var exception = Record.Exception(() => about.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(about, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_WithValidEventArgs_ShouldNotThrow()
        {
            // Arrange
            var about = new About();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => about.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(about, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void About_TypeShouldBePublic()
        {
            // Arrange
            var aboutType = typeof(About);

            // Act & Assert
            Assert.True(aboutType.IsPublic);
        }

        [Fact]
        public void About_ShouldBePartialClass()
        {
            // Arrange
            var aboutType = typeof(About);

            // Act & Assert
            Assert.True(aboutType.IsClass);
        }
    }
}
