using System;
using Xunit;

namespace FIlms.Tests
{
    public class AboutDesignerTests
    {
        [Fact]
        public void About_DesignerPartial_ShouldCreateInstance()
        {
            // Arrange & Act
            var about = new About();

            // Assert
            Assert.NotNull(about);
        }

        [Fact]
        public void About_DesignerClass_ShouldBePublic()
        {
            // Arrange
            var aboutType = typeof(About);

            // Act & Assert
            Assert.True(aboutType.IsPublic);
        }

        [Fact]
        public void About_DesignerClass_ShouldBeClass()
        {
            // Arrange
            var aboutType = typeof(About);

            // Act & Assert
            Assert.True(aboutType.IsClass);
        }

        [Fact]
        public void About_DesignerClass_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var aboutType = typeof(About);

            // Act & Assert
            Assert.Equal("FIlms", aboutType.Namespace);
        }
    }
}
