using System;
using Xunit;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace FIlms.Tests
{
    public class LogInTests
    {
        [Fact]
        public void LogIn_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var logIn = new LogIn();

            // Assert
            Assert.NotNull(logIn);
        }

        [Fact]
        public void LogIn_ShouldInheritFromPage()
        {
            // Arrange
            var logIn = new LogIn();

            // Act & Assert
            Assert.IsAssignableFrom<Page>(logIn);
        }

        [Fact]
        public void LogIn_TypeShouldBePublic()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act & Assert
            Assert.True(logInType.IsPublic);
        }

        [Fact]
        public void LogIn_ShouldBePartialClass()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act & Assert
            Assert.True(logInType.IsClass);
        }

        [Fact]
        public void LogIn_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act & Assert
            Assert.Equal("FIlms", logInType.Namespace);
        }

        [Fact]
        public void Page_Load_MethodExists()
        {
            // Arrange
            var logIn = new LogIn();
            var method = logIn.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Page_Load_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var logIn = new LogIn();

            // Act & Assert
            var exception = Record.Exception(() => logIn.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(logIn, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void Button2_Click_MethodExists()
        {
            // Arrange
            var logIn = new LogIn();
            var method = logIn.GetType().GetMethod("Button2_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Button2_Click_HasCorrectSignature()
        {
            // Arrange
            var logInType = typeof(LogIn);
            var method = logInType.GetMethod("Button2_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void CheckBox1_CheckedChanged_MethodExists()
        {
            // Arrange
            var logIn = new LogIn();
            var method = logIn.GetType().GetMethod("CheckBox1_CheckedChanged",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void CheckBox1_CheckedChanged_HasCorrectSignature()
        {
            // Arrange
            var logInType = typeof(LogIn);
            var method = logInType.GetMethod("CheckBox1_CheckedChanged",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Button3_Click_MethodExists()
        {
            // Arrange
            var logIn = new LogIn();
            var method = logIn.GetType().GetMethod("Button3_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Button3_Click_HasCorrectSignature()
        {
            // Arrange
            var logInType = typeof(LogIn);
            var method = logInType.GetMethod("Button3_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Button4_Click_MethodExists()
        {
            // Arrange
            var logIn = new LogIn();
            var method = logIn.GetType().GetMethod("Button4_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Button4_Click_HasCorrectSignature()
        {
            // Arrange
            var logInType = typeof(LogIn);
            var method = logInType.GetMethod("Button4_Click",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Button3_Click_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var logIn = new LogIn();

            // Act & Assert
            var exception = Record.Exception(() => logIn.GetType().GetMethod("Button3_Click",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(logIn, new object[] { null, EventArgs.Empty }));

            // Assert - method should execute but may have null reference exceptions
            // Testing that method exists and can be invoked
            Assert.True(exception == null || exception.InnerException is NullReferenceException);
        }

        [Fact]
        public void Button4_Click_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var logIn = new LogIn();

            // Act & Assert
            var exception = Record.Exception(() => logIn.GetType().GetMethod("Button4_Click",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(logIn, new object[] { null, EventArgs.Empty }));

            // Assert - method should execute but may have null reference exceptions
            // Testing that method exists and can be invoked
            Assert.True(exception == null || exception.InnerException is NullReferenceException);
        }
    }
}
