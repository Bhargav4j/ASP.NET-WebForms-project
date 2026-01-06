using System;
using Xunit;
using System.Web;
using System.Reflection;

namespace FIlms.Tests
{
    public class GlobalTests
    {
        [Fact]
        public void Global_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var global = new Global();

            // Assert
            Assert.NotNull(global);
        }

        [Fact]
        public void Global_ShouldInheritFromHttpApplication()
        {
            // Arrange
            var global = new Global();

            // Act & Assert
            Assert.IsAssignableFrom<HttpApplication>(global);
        }

        [Fact]
        public void Global_TypeShouldBePublic()
        {
            // Arrange
            var globalType = typeof(Global);

            // Act & Assert
            Assert.True(globalType.IsPublic);
        }

        [Fact]
        public void Global_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var globalType = typeof(Global);

            // Act & Assert
            Assert.Equal("FIlms", globalType.Namespace);
        }

        [Fact]
        public void Application_Start_MethodExists()
        {
            // Arrange
            var global = new Global();
            var method = global.GetType().GetMethod("Application_Start",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Application_Start_HasCorrectSignature()
        {
            // Arrange
            var globalType = typeof(Global);
            var method = globalType.GetMethod("Application_Start",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Application_End_MethodExists()
        {
            // Arrange
            var global = new Global();
            var method = global.GetType().GetMethod("Application_End",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Application_End_HasCorrectSignature()
        {
            // Arrange
            var globalType = typeof(Global);
            var method = globalType.GetMethod("Application_End",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Application_Error_MethodExists()
        {
            // Arrange
            var global = new Global();
            var method = global.GetType().GetMethod("Application_Error",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Application_Error_HasCorrectSignature()
        {
            // Arrange
            var globalType = typeof(Global);
            var method = globalType.GetMethod("Application_Error",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Application_End_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var global = new Global();

            // Act & Assert
            var exception = Record.Exception(() => global.GetType().GetMethod("Application_End",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(global, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }

        [Fact]
        public void Application_Error_WithNullSender_ShouldNotThrow()
        {
            // Arrange
            var global = new Global();

            // Act & Assert
            var exception = Record.Exception(() => global.GetType().GetMethod("Application_Error",
                BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(global, new object[] { null, EventArgs.Empty }));

            Assert.Null(exception);
        }
    }
}
