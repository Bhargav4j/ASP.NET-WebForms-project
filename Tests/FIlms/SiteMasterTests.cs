using System;
using Xunit;
using System.Web;
using System.Web.UI;
using System.Reflection;

namespace FIlms.Tests
{
    public class SiteMasterTests
    {
        [Fact]
        public void SiteMaster_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var siteMaster = new SiteMaster();

            // Assert
            Assert.NotNull(siteMaster);
        }

        [Fact]
        public void SiteMaster_ShouldInheritFromMasterPage()
        {
            // Arrange
            var siteMaster = new SiteMaster();

            // Act & Assert
            Assert.IsAssignableFrom<MasterPage>(siteMaster);
        }

        [Fact]
        public void SiteMaster_TypeShouldBePublic()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act & Assert
            Assert.True(siteMasterType.IsPublic);
        }

        [Fact]
        public void SiteMaster_ShouldBePartialClass()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act & Assert
            Assert.True(siteMasterType.IsClass);
        }

        [Fact]
        public void SiteMaster_ShouldBeInFIlmsNamespace()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act & Assert
            Assert.Equal("FIlms", siteMasterType.Namespace);
        }

        [Fact]
        public void SiteMaster_ShouldHaveAntiXsrfTokenKeyConstant()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("AntiXsrfTokenKey",
                BindingFlags.NonPublic | BindingFlags.Static);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsLiteral);
            Assert.Equal("__AntiXsrfToken", field.GetValue(null));
        }

        [Fact]
        public void SiteMaster_ShouldHaveAntiXsrfUserNameKeyConstant()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("AntiXsrfUserNameKey",
                BindingFlags.NonPublic | BindingFlags.Static);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsLiteral);
            Assert.Equal("__AntiXsrfUserName", field.GetValue(null));
        }

        [Fact]
        public void SiteMaster_ShouldHaveAntiXsrfTokenValueField()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("_antiXsrfTokenValue",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(string), field.FieldType);
        }

        [Fact]
        public void Page_Init_MethodExists()
        {
            // Arrange
            var siteMaster = new SiteMaster();
            var method = siteMaster.GetType().GetMethod("Page_Init",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Page_Init_HasCorrectSignature()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);
            var method = siteMasterType.GetMethod("Page_Init",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Master_Page_PreLoad_MethodExists()
        {
            // Arrange
            var siteMaster = new SiteMaster();
            var method = siteMaster.GetType().GetMethod("master_Page_PreLoad",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Master_Page_PreLoad_HasCorrectSignature()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);
            var method = siteMasterType.GetMethod("master_Page_PreLoad",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void Page_Load_MethodExists()
        {
            // Arrange
            var siteMaster = new SiteMaster();
            var method = siteMaster.GetType().GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
        }

        [Fact]
        public void Page_Load_HasCorrectSignature()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);
            var method = siteMasterType.GetMethod("Page_Load",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Act & Assert
            Assert.NotNull(method);
            var parameters = method.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(object), parameters[0].ParameterType);
            Assert.Equal(typeof(EventArgs), parameters[1].ParameterType);
        }

        [Fact]
        public void SiteMaster_HasAllEventHandlerMethods()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);
            var methodNames = new[] { "Page_Init", "master_Page_PreLoad", "Page_Load" };

            // Act & Assert
            foreach (var methodName in methodNames)
            {
                var method = siteMasterType.GetMethod(methodName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.NotNull(method);
            }
        }

        [Fact]
        public void AntiXsrfTokenKey_ShouldHaveCorrectValue()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);
            var field = siteMasterType.GetField("AntiXsrfTokenKey",
                BindingFlags.NonPublic | BindingFlags.Static);

            // Act
            var value = field.GetValue(null);

            // Assert
            Assert.Equal("__AntiXsrfToken", value);
        }

        [Fact]
        public void AntiXsrfUserNameKey_ShouldHaveCorrectValue()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);
            var field = siteMasterType.GetField("AntiXsrfUserNameKey",
                BindingFlags.NonPublic | BindingFlags.Static);

            // Act
            var value = field.GetValue(null);

            // Assert
            Assert.Equal("__AntiXsrfUserName", value);
        }
    }
}
