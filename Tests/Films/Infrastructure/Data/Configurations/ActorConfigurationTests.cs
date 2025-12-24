using Films.Domain.Entities;
using Films.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq;
using Xunit;

namespace Films.Tests.Infrastructure.Data.Configurations
{
    public class ActorConfigurationTests
    {
        private readonly ModelBuilder _modelBuilder;
        private readonly ActorConfiguration _configuration;
        private IModel _model;

        public ActorConfigurationTests()
        {
            var conventionSet = new ConventionSet();
            _modelBuilder = new ModelBuilder(conventionSet);
            _configuration = new ActorConfiguration();
            _configuration.Configure(_modelBuilder.Entity<Actor>());
            _model = _modelBuilder.FinalizeModel();
        }

        [Fact]
        public void ActorEntity_ShouldMapToCorrectTableName()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Actor));

            // Assert
            Assert.Equal("Actor", entityType.GetTableName());
        }

        [Fact]
        public void ActorEntity_ShouldHavePrimaryKey()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Actor));

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
            var entityType = _model.FindEntityType(typeof(Actor));

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
            var entityType = _model.FindEntityType(typeof(Actor));

            // Act
            var property = entityType.FindProperty("Name");

            // Assert
            Assert.NotNull(property);
            Assert.Equal(100, property.GetMaxLength());
        }

        [Fact]
        public void Surname_ShouldBeOptional()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Actor));

            // Act
            var property = entityType.FindProperty("Surname");

            // Assert
            Assert.NotNull(property);
            Assert.True(property.IsNullable);
        }

        [Fact]
        public void Surname_ShouldHaveMaxLength()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Actor));

            // Act
            var property = entityType.FindProperty("Surname");

            // Assert
            Assert.NotNull(property);
            Assert.Equal(100, property.GetMaxLength());
        }

        [Fact]
        public void CreatedDate_ShouldBeRequired()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Actor));

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
            var entityType = _model.FindEntityType(typeof(Actor));

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
            var entityType = _model.FindEntityType(typeof(Actor));

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
            var entityType = _model.FindEntityType(typeof(Actor));

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
            var entityType = _model.FindEntityType(typeof(Actor));

            // Act
            var property = entityType.FindProperty("ModifiedBy");

            // Assert
            Assert.NotNull(property);
            Assert.True(property.IsNullable);
            Assert.Equal(100, property.GetMaxLength());
        }

        [Fact]
        public void Sex_ShouldHaveRestrictDelete()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Actor));

            // Act
            var navigation = entityType.FindNavigation(nameof(Actor.Sex));

            // Assert
            Assert.NotNull(navigation);
            Assert.Equal(DeleteBehavior.Restrict, navigation.ForeignKey.DeleteBehavior);
        }

        [Fact]
        public void RefAFs_ShouldHaveCascadeDelete()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Actor));

            // Act
            var navigation = entityType.FindNavigation(nameof(Actor.RefAFs));

            // Assert
            Assert.NotNull(navigation);
            Assert.Equal(DeleteBehavior.Cascade, navigation.ForeignKey.DeleteBehavior);
        }

        [Fact]
        public void IdSex_ShouldBeForeignKey()
        {
            // Arrange
            var entityType = _model.FindEntityType(typeof(Actor));

            // Act
            var fk = entityType.FindProperty("IdSex");

            // Assert
            Assert.NotNull(fk);
            Assert.True(fk.IsNullable);
        }
    }
}