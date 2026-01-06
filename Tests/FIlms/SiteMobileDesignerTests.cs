using System;
using Xunit;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace FIlms.Tests
{
    public class SiteMobileDesignerTests
    {
        [Fact]
        public void Site_Mobile_DesignerPartial_ShouldCreateInstance()
        {
            // Arrange & Act
            var siteMobile = new Site_Mobile();

            // Assert
            Assert.NotNull(siteMobile);
        }

        [Fact]
        public void Site_Mobile_ShouldHaveHeadContentField()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act
            var field = siteMobileType.GetField("HeadContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(ContentPlaceHolder), field.FieldType);
        }

        [Fact]
        public void Site_Mobile_ShouldHaveForm1Field()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act
            var field = siteMobileType.GetField("form1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(HtmlForm), field.FieldType);
        }

        [Fact]
        public void Site_Mobile_ShouldHaveFeaturedContentField()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act
            var field = siteMobileType.GetField("FeaturedContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(ContentPlaceHolder), field.FieldType);
        }

        [Fact]
        public void Site_Mobile_ShouldHaveMainContentField()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act
            var field = siteMobileType.GetField("MainContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.Equal(typeof(ContentPlaceHolder), field.FieldType);
        }

        [Fact]
        public void Site_Mobile_HeadContentField_ShouldBeProtected()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act
            var field = siteMobileType.GetField("HeadContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }

        [Fact]
        public void Site_Mobile_Form1Field_ShouldBeProtected()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act
            var field = siteMobileType.GetField("form1",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }

        [Fact]
        public void Site_Mobile_FeaturedContentField_ShouldBeProtected()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act
            var field = siteMobileType.GetField("FeaturedContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }

        [Fact]
        public void Site_Mobile_MainContentField_ShouldBeProtected()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);

            // Act
            var field = siteMobileType.GetField("MainContent",
                BindingFlags.NonPublic | BindingFlags.Instance);

            // Assert
            Assert.NotNull(field);
            Assert.True(field.IsFamily);
        }

        [Fact]
        public void Site_Mobile_AllContentPlaceHolders_ShouldExist()
        {
            // Arrange
            var siteMobileType = typeof(Site_Mobile);
            var contentPlaceHolderNames = new[] { "HeadContent", "FeaturedContent", "MainContent" };

            // Act & Assert
            foreach (var name in contentPlaceHolderNames)
            {
                var field = siteMobileType.GetField(name,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                Assert.NotNull(field);
                Assert.Equal(typeof(ContentPlaceHolder), field.FieldType);
            }
        }
    }
}
