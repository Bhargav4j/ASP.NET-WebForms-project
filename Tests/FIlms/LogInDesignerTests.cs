using System;
using Xunit;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace FIlms.Tests
{
    public class LogInDesignerTests
    {
        [Fact]
        public void LogIn_DesignerPartial_ShouldCreateInstance()
        {
            // Arrange & Act
            var logIn = new LogIn();

            // Assert
            Assert.NotNull(logIn);
        }

        [Fact]
        public void LogIn_ShouldHaveForm1Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("form1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(HtmlForm), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHavePanel1Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("Panel1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Panel), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveTextBox1Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("TextBox1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveTextBox2Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("TextBox2",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveButton1Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("Button1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Button), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveButton2Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("Button2",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Button), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveCheckBox1Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("CheckBox1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(CheckBox), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHavePanel2Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("Panel2",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Panel), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHavePanel3Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("Panel3",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Panel), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveTextBox3Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("TextBox3",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveButton3Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("Button3",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Button), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveButton4Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("Button4",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Button), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveLabel4Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("Label4",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Label), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveTextBox4Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("TextBox4",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveRequiredFieldValidator1Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("RequiredFieldValidator1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(RequiredFieldValidator), field.FieldType);
        }

        [Fact]
        public void LogIn_ShouldHaveRequiredFieldValidator2Field()
        {
            // Arrange
            var logInType = typeof(LogIn);

            // Act
            var field = logInType.GetField("RequiredFieldValidator2",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(RequiredFieldValidator), field.FieldType);
        }

        [Fact]
        public void LogIn_AllProtectedFields_ShouldBeProtected()
        {
            // Arrange
            var logInType = typeof(LogIn);
            var fieldNames = new[] { "form1", "Panel1", "TextBox1", "Button1", "CheckBox1" };

            // Act & Assert
            foreach (var fieldName in fieldNames)
            {
                var field = logInType.GetField(fieldName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.NotNull(field);
                Assert.True(field.IsFamily);
            }
        }
    }
}
