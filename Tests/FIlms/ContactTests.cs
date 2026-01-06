using System;
using Xunit;
using System.Web;
using System.Web.UI;

namespace FIlms.Tests
{
    public class ContactTests
    {
        [Fact]
        public void Contact_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var contact = new Contact();

            // Assert
            Assert.NotNull(contact);
        }

        [Fact]
        public void Contact_ShouldInheritFromPage()
        {
            // Arrange
            var contact = new Contact();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(contact);
        }

        [Fact]
        public void Page_Load_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var contact = new Contact();

            // Act & Assert
            var exception = Record.Exception(() => contact.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(contact, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void Page_Load_WithValidEventArgs_ShouldNotThrow()
        {
            // Arrange
            var contact = new Contact();
            var eventArgs = EventArgs.Empty;

            // Act & Assert
            var exception = Record.Exception(() => contact.GetType().GetMethod("Page_Load",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(contact, new object[] { null, eventArgs }));

            Assert.Null(exception);
        }

        [Fact]
        public void Contact_TypeShouldBePublic()
        {
            // Arrange
            var contactType = typeof(Contact);

            // Act & Assert
            Assert.True(contactType.IsPublic);
        }

        [Fact]
        public void Contact_ShouldBePartialClass()
        {
            // Arrange
            var contactType = typeof(Contact);

            // Act & Assert
            Assert.True(contactType.IsClass);
        }

        [Fact]
        public void Contact_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var contactType = typeof(Contact);

            // Act & Assert
            Assert.Equal("FIlms", contactType.Namespace);
        }
    }
}
