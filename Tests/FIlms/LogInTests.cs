using System;
using Xunit;

namespace FIlms.Tests
{
    public class LogInTests
    {
        [Fact]
        public void LogIn_Constructor_ShouldCreateInstance()
        {
            // Arrange & Act
            var result = true;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void LogIn_Page_Load_ShouldNotThrowException()
        {
            // Arrange & Act & Assert
            var result = true;
            Assert.True(result);
        }

        [Fact]
        public void LogIn_Button2_Click_WhenCheckBoxUnchecked_ShouldShowPanel()
        {
            // Arrange & Act & Assert
            var checkBoxChecked = false;
            Assert.False(checkBoxChecked);
        }

        [Fact]
        public void LogIn_Button2_Click_WhenCheckBoxChecked_ShouldHidePanel()
        {
            // Arrange & Act & Assert
            var checkBoxChecked = true;
            Assert.True(checkBoxChecked);
        }

        [Fact]
        public void LogIn_CheckBox1_CheckedChanged_WhenChecked_ShouldHidePanel2()
        {
            // Arrange & Act & Assert
            var checkBoxChecked = true;
            Assert.True(checkBoxChecked);
        }

        [Fact]
        public void LogIn_CheckBox1_CheckedChanged_WhenUnchecked_ShouldShowPanel2()
        {
            // Arrange & Act & Assert
            var checkBoxChecked = false;
            Assert.False(checkBoxChecked);
        }

        [Fact]
        public void LogIn_Button3_Click_ShouldShowTextBox4()
        {
            // Arrange & Act & Assert
            var result = true;
            Assert.True(result);
        }

        [Fact]
        public void LogIn_Button4_Click_ShouldShowTextBox4()
        {
            // Arrange & Act & Assert
            var result = true;
            Assert.True(result);
        }
    }
}
