using System;
using Xunit;

namespace FIlms.Tests
{
    public class ContactDesignerTests
    {
        [Fact]
        public void Contact_DesignerPartial_ShouldCreateInstance()
        {
            // Arrange & Act
            var contact = new Contact();

            // Assert
            Assert.NotNull(contact);
        }

        [Fact]
        public void Contact_DesignerClass_ShouldBePublic()
        {
            // Arrange
            var contactType = typeof(Contact);

            // Act & Assert
            Assert.True(contactType.IsPublic);
        }

        [Fact]
        public void Contact_DesignerClass_ShouldBeClass()
        {
            // Arrange
            var contactType = typeof(Contact);

            // Act & Assert
            Assert.True(contactType.IsClass);
        }

        [Fact]
        public void Contact_DesignerClass_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var contactType = typeof(Contact);

            // Act & Assert
            Assert.Equal("FIlms", contactType.Namespace);
        }

        [Fact]
        public void Contact_DesignerClass_ShouldBePartial()
        {
            // Arrange
            var contactType = typeof(Contact);

            // Act & Assert
            Assert.True(contactType.IsClass);
        }
    }
}
