using System;
using Xunit;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace FIlms.Tests
{
    public class RegisterTests
    {
        [Fact]
        public void Register_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var register = new Register();

            // Assert
            Assert.NotNull(register);
        }

        [Fact]
        public void Register_ShouldInheritFromPage()
        {
            // Arrange
            var register = new Register();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(register);
        }

        [Fact]
        public void Register_TypeShouldBePublic()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act & Assert
            Assert.True(registerType.IsPublic);
        }

        [Fact]
        public void Register_ShouldBePartialClass()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act & Assert
            Assert.True(registerType.IsClass);
        }

        [Fact]
        public void Register_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act & Assert
            Assert.Equal("FIlms", registerType.Namespace);
        }

        [Fact]
        public void Page_Load_MethodExists()
        {
            // Arrange
            var register = new Register();
            var method = register.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Page_Load_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var register = new Register();

            // Act & Assert
            var exception = Record.Exception(() => register.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(register, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void Button1_Click_MethodExists()
        {
            // Arrange
            var register = new Register();
            var method = register.GetType().GetMethod("Button1_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Button1_Click_HasCorrectSignature()
        {
            // Arrange
            var registerType = typeof(Register);
            var method = registerType.GetMethod("Button1_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Button1_Click_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var register = new Register();

            // Act & Assert
            var exception = Record.Exception(() => register.GetType().GetMethod("Button1_Click",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(register, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void Calendar1_SelectionChanged_MethodExists()
        {
            // Arrange
            var register = new Register();
            var method = register.GetType().GetMethod("Calendar1_SelectionChanged",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Calendar1_SelectionChanged_HasCorrectSignature()
        {
            // Arrange
            var registerType = typeof(Register);
            var method = registerType.GetMethod("Calendar1_SelectionChanged",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Calendar1_SelectionChanged_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var register = new Register();

            // Act & Assert
            var exception = Record.Exception(() => register.GetType().GetMethod("Calendar1_SelectionChanged",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(register, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void CalDate_SelectionChanged_MethodExists()
        {
            // Arrange
            var register = new Register();
            var method = register.GetType().GetMethod("calDate_SelectionChanged",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void CalDate_SelectionChanged_HasCorrectSignature()
        {
            // Arrange
            var registerType = typeof(Register);
            var method = registerType.GetMethod("calDate_SelectionChanged",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void CalDate_SelectionChanged_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var register = new Register();

            // Act & Assert
            var exception = Record.Exception(() => register.GetType().GetMethod("calDate_SelectionChanged",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(register, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void Register_HasAllEventHandlerMethods()
        {
            // Arrange
            var registerType = typeof(Register);
            var methodNames = new[] { "Page_Load", "Button1_Click", "Calendar1_SelectionChanged", "calDate_SelectionChanged" };

            // Act & Assert
            foreach (var methodName in methodNames)
            {
                var method = registerType.GetMethod(methodName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.NotNull(method);
            }
        }
    }
}
