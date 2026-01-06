using System;
using Xunit;
using System.Web.UI.WebControls;

namespace FIlms.Tests
{
    public class DefaultDesignerTests
    {
        [Fact]
        public void Default_DesignerPartial_ShouldCreateInstance()
        {
            // Arrange & Act
            var defaultPage = new _Default();

            // Assert
            Assert.NotNull(defaultPage);
        }

        [Fact]
        public void Default_DesignerClass_ShouldBePublic()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act & Assert
            Assert.True(defaultType.IsPublic);
        }

        [Fact]
        public void Default_DesignerClass_ShouldBeClass()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act & Assert
            Assert.True(defaultType.IsClass);
        }

        [Fact]
        public void Default_DesignerClass_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act & Assert
            Assert.Equal("FIlms", defaultType.Namespace);
        }

        [Fact]
        public void Default_ShouldHaveImage1Field()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act
            var field = defaultType.GetField("Image1",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Image), field.FieldType);
        }

        [Fact]
        public void Default_ShouldHaveButton1Field()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act
            var field = defaultType.GetField("Button1",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Button), field.FieldType);
        }

        [Fact]
        public void Default_Image1Field_ShouldBeProtected()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act
            var field = defaultType.GetField("Image1",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }

        [Fact]
        public void Default_Button1Field_ShouldBeProtected()
        {
            // Arrange
            var defaultType = typeof(_Default);

            // Act
            var field = defaultType.GetField("Button1",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }
    }
}
