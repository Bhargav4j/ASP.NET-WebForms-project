using Films.Domain.Entities;
using Films.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq;
using Xunit;

namespace Films.Tests.Infrastructure.Data.Configurations
{
    public class FilmConfigurationTests
    {
        private readonly ModelBuilder _modelBuilder;
        private readonly FilmConfiguration _configuration;
        private IModel _model;

        public FilmConfigurationTests()
        {
            var conventionSet = new ConventionSet();
            _modelBuilder = new ModelBuilder(conventionSet);
            _configuration = new FilmConfiguration();
            _configuration.Configure(_modelBuilder.Entity<Film>());
            _model = _modelBuilder.FinalizeModel();
        }

        [Fact]
        public void FilmEntity_ShouldMapToCorrectTableName()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Assert
            Assert.Equal("Film", entityType.GetTableName());
        }

        [Fact]
        public void FilmEntity_ShouldHavePrimaryKey()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var primaryKey = entityType.FindPrimaryKey();

            // Assert
            Assert.NotNull(primaryKey);
            Assert.Single(primaryKey.Properties);
            Assert.Equal("Id", primaryKey.Properties.Single().Name);
        }

        [Fact]
        public void Name_ShouldBeRequired()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var property = entityType.FindProperty("Name");

            // Assert
            Assert.NotNull(property);
            Assert.False(property.IsNullable);
        }

        [Fact]
        public void Name_ShouldHaveMaxLength()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var property = entityType.FindProperty("Name");

            // Assert
            Assert.NotNull(property);
            Assert.Equal(200, property.GetMaxLength());
        }

        [Fact]
        public void Description_ShouldBeOptional()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var property = entityType.FindProperty("Description");

            // Assert
            Assert.NotNull(property);
            Assert.True(property.IsNullable);
        }

        [Fact]
        public void Description_ShouldHaveMaxLength()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var property = entityType.FindProperty("Description");

            // Assert
            Assert.NotNull(property);
            Assert.Equal(1000, property.GetMaxLength());
        }

        [Fact]
        public void CreatedDate_ShouldBeRequired()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var property = entityType.FindProperty("CreatedDate");

            // Assert
            Assert.NotNull(property);
            Assert.False(property.IsNullable);
        }

        [Fact]
        public void IsActive_ShouldBeRequired()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var property = entityType.FindProperty("IsActive");

            // Assert
            Assert.NotNull(property);
            Assert.False(property.IsNullable);
        }

        [Fact]
        public void IsActive_ShouldHaveDefaultValue()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var property = entityType.FindProperty("IsActive");

            // Assert
            Assert.NotNull(property);
            Assert.Equal(true, property.GetDefaultValue());
        }

        [Fact]
        public void CreatedBy_ShouldBeRequired()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var property = entityType.FindProperty("CreatedBy");

            // Assert
            Assert.NotNull(property);
            Assert.False(property.IsNullable);
            Assert.Equal(100, property.GetMaxLength());
        }

        [Fact]
        public void ModifiedBy_ShouldBeOptional()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var property = entityType.FindProperty("ModifiedBy");

            // Assert
            Assert.NotNull(property);
            Assert.True(property.IsNullable);
            Assert.Equal(100, property.GetMaxLength());
        }

        [Fact]
        public void RefAFs_ShouldHaveCascadeDelete()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var navigation = entityType.FindNavigation(nameof(Film.RefAFs));

            // Assert
            Assert.NotNull(navigation);
            Assert.Equal(DeleteBehavior.Cascade, navigation.ForeignKey.DeleteBehavior);
        }

        [Fact]
        public void RefDAFs_ShouldHaveCascadeDelete()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Film));

            // Act
            var navigation = entityType.FindNavigation(nameof(Film.RefDAFs));

            // Assert
            Assert.NotNull(navigation);
            Assert.Equal(DeleteBehavior.Cascade, navigation.ForeignKey.DeleteBehavior);
        }
    }
}