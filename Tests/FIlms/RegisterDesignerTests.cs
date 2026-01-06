using System;
using Xunit;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace FIlms.Tests
{
    public class RegisterDesignerTests
    {
        [Fact]
        public void Register_DesignerPartial_ShouldCreateInstance()
        {
            // Arrange & Act
            var register = new Register();

            // Assert
            Assert.NotNull(register);
        }

        [Fact]
        public void Register_ShouldHaveForm1Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("form1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(HtmlForm), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHavePanel1Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("Panel1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Panel), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveTextBox1Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("TextBox1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveTextBox2Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("TextBox2",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveTextBox3Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("TextBox3",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveTextBox4Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("TextBox4",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveTextBox5Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("TextBox5",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveTextBox6Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("TextBox6",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveTextBox7Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("TextBox7",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(TextBox), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveButton1Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("Button1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Button), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveCalendar1Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("Calendar1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Calendar), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveDropDownList1Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("DropDownList1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(DropDownList), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveRequiredFieldValidator1Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("RequiredFieldValidator1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(RequiredFieldValidator), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveRequiredFieldValidator2Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("RequiredFieldValidator2",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(RequiredFieldValidator), field.FieldType);
        }

        [Fact]
        public void Register_ShouldHaveRequiredFieldValidator3Field()
        {
            // Arrange
            var registerType = typeof(Register);

            // Act
            var field = registerType.GetField("RequiredFieldValidator3",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(RequiredFieldValidator), field.FieldType);
        }

        [Fact]
        public void Register_AllProtectedFields_ShouldBeProtected()
        {
            // Arrange
            var registerType = typeof(Register);
            var fieldNames = new[] { "form1", "Panel1", "TextBox1", "Button1", "Calendar1" };

            // Act & Assert
            foreach (var fieldName in fieldNames)
            {
                var field = registerType.GetField(fieldName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.NotNull(field);
                Assert.True(field.IsFamily);
            }
        }

        [Fact]
        public void Register_ShouldHaveAllTextBoxes()
        {
            // Arrange
            var registerType = typeof(Register);
            var textBoxNames = new[] { "TextBox1", "TextBox2", "TextBox3", "TextBox4", "TextBox5", "TextBox6", "TextBox7" };

            // Act & Assert
            foreach (var textBoxName in textBoxNames)
            {
                var field = registerType.GetField(textBoxName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.NotNull(field);
                Assert.Equal(typeof(TextBox), field.FieldType);
            }
        }
    }
}
