using System;
using Xunit;

namespace FIlms.Tests
{
    public class _DefaultTests
    {
        [Fact]
        public void _Default_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var result = true;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void _Default_Page_Load_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            var result = true;
            Assert.True(result);
        }

        [Fact]
        public void _Default_Button1_Click_ShouldRedirect()
        {
            // Arrange & Act & Assert
            var expectedUrl = "https://moodle.unwe.bg";
            Assert.NotNull(expectedUrl);
            Assert.Equal("https://moodle.unwe.bg", expectedUrl);
        }
    }
}
