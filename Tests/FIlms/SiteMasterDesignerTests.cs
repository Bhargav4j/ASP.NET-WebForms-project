using System;
using Xunit;
using System.Web.UI.WebControls;
using System.Reflection;

namespace FIlms.Tests
{
    public class SiteMasterDesignerTests
    {
        [Fact]
        public void SiteMaster_DesignerPartial_ShouldCreateInstance()
        {
            // Arrange & Act
            var siteMaster = new SiteMaster();

            // Assert
            Assert.NotNull(siteMaster);
        }

        [Fact]
        public void SiteMaster_ShouldHaveHeadContentField()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("HeadContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(ContentPlaceHolder), field.FieldType);
        }

        [Fact]
        public void SiteMaster_ShouldHaveLabel1Field()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("Label1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(Label), field.FieldType);
        }

        [Fact]
        public void SiteMaster_ShouldHaveFeaturedContentField()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("FeaturedContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(ContentPlaceHolder), field.FieldType);
        }

        [Fact]
        public void SiteMaster_ShouldHaveMainContentField()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("MainContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(ContentPlaceHolder), field.FieldType);
        }

        [Fact]
        public void SiteMaster_HeadContentField_ShouldBeProtected()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("HeadContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }

        [Fact]
        public void SiteMaster_Label1Field_ShouldBeProtected()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("Label1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }

        [Fact]
        public void SiteMaster_FeaturedContentField_ShouldBeProtected()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("FeaturedContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }

        [Fact]
        public void SiteMaster_MainContentField_ShouldBeProtected()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);

            // Act
            var field = siteMasterType.GetField("MainContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }

        [Fact]
        public void SiteMaster_AllContentPlaceHolders_ShouldExist()
        {
            // Arrange
            var siteMasterType = typeof(SiteMaster);
            var contentPlaceHolderNames = new[] { "HeadContent", "FeaturedContent", "MainContent" };

            // Act & Assert
            foreach (var name in contentPlaceHolderNames)
            {
                var field = siteMasterType.GetField(name,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.NotNull(field);
                Assert.Equal(typeof(ContentPlaceHolder), field.FieldType);
            }
        }
    }
}
