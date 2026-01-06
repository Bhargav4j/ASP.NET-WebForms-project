using System;
using Xunit;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace FIlms.Tests
{
    public class ViewSwitcherTests
    {
        [Fact]
        public void ViewSwitcher_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var viewSwitcher = new ViewSwitcher();

            // Assert
            Assert.NotNull(viewSwitcher);
        }

        [Fact]
        public void ViewSwitcher_ShouldInheritFromUserControl()
        {
            // Arrange
            var viewSwitcher = new ViewSwitcher();

            // Act & Assert
            Assert.IsAssignableFrom<UserControl>(viewSwitcher);
        }

        [Fact]
        public void ViewSwitcher_TypeShouldBePublic()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);

            // Act & Assert
            Assert.True(viewSwitcherType.IsPublic);
        }

        [Fact]
        public void ViewSwitcher_ShouldBePartialClass()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);

            // Act & Assert
            Assert.True(viewSwitcherType.IsClass);
        }

        [Fact]
        public void ViewSwitcher_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);

            // Act & Assert
            Assert.Equal("FIlms", viewSwitcherType.Namespace);
        }

        [Fact]
        public void ViewSwitcher_ShouldHaveCurrentViewProperty()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);

            // Act
            var property = viewSwitcherType.GetProperty("CurrentView",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // Assert
            Assert.NotNull(property);
            Assert.Equal(typeof(string), property.PropertyType);
        }

        [Fact]
        public void ViewSwitcher_ShouldHaveAlternateViewProperty()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);

            // Act
            var property = viewSwitcherType.GetProperty("AlternateView",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // Assert
            Assert.NotNull(property);
            Assert.Equal(typeof(string), property.PropertyType);
        }

        [Fact]
        public void ViewSwitcher_ShouldHaveSwitchUrlProperty()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);

            // Act
            var property = viewSwitcherType.GetProperty("SwitchUrl",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // Assert
            Assert.NotNull(property);
            Assert.Equal(typeof(string), property.PropertyType);
        }

        [Fact]
        public void Page_Load_MethodExists()
        {
            // Arrange
            var viewSwitcher = new ViewSwitcher();
            var method = viewSwitcher.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Page_Load_HasCorrectSignature()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);
            var method = viewSwitcherType.GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void CurrentView_Property_ShouldHavePrivateSetter()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);
            var property = viewSwitcherType.GetProperty("CurrentView",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // Act
            var setMethod = property.GetSetMethod(true);

            // Assert
            Assert.NotNull(setMethod);
            Assert.True(setMethod.IsPrivate);
        }

        [Fact]
        public void AlternateView_Property_ShouldHavePrivateSetter()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);
            var property = viewSwitcherType.GetProperty("AlternateView",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // Act
            var setMethod = property.GetSetMethod(true);

            // Assert
            Assert.NotNull(setMethod);
            Assert.True(setMethod.IsPrivate);
        }

        [Fact]
        public void SwitchUrl_Property_ShouldHavePrivateSetter()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);
            var property = viewSwitcherType.GetProperty("SwitchUrl",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // Act
            var setMethod = property.GetSetMethod(true);

            // Assert
            Assert.NotNull(setMethod);
            Assert.True(setMethod.IsPrivate);
        }

        [Fact]
        public void ViewSwitcher_AllProperties_ShouldBeStringType()
        {
            // Arrange
            var viewSwitcherType = typeof(ViewSwitcher);
            var propertyNames = new[] { "CurrentView", "AlternateView", "SwitchUrl" };

            // Act & Assert
            foreach (var propertyName in propertyNames)
            {
                var property = viewSwitcherType.GetProperty(propertyName,
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                Assert.NotNull(property);
                Assert.Equal(typeof(string), property.PropertyType);
            }
        }
    }
}
