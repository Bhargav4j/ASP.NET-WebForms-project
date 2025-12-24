using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Films.Application.DTOs;
using Films.Application.Interfaces;
using Films.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Films.Tests.Web.Controllers
{
    public class ActorsControllerTests
    {
        private readonly Mock<IActorService> _mockActorService;
        private readonly ActorsController _controller;

        public ActorsControllerTests()
        {
            _mockActorService = new Mock<IActorService>();
            _controller = new ActorsController(_mockActorService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithListOfActors()
        {
            // Arrange
            var actors = new List<ActorDto>
            {
                new ActorDto { Id = 1, Name = "John", Surname = "Doe", SexName = "Male" },
                new ActorDto { Id = 2, Name = "Jane", Surname = "Smith", SexName = "Female" }
            };

            _mockActorService.Setup(service => service.GetAllAsync(default))
                .ReturnsAsync(actors);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedActors = Assert.IsAssignableFrom<IEnumerable<ActorDto>>(okResult.Value);
            Assert.Equal(2, returnedActors.Count());
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnOkResult_WithActor()
        {
            // Arrange
            var actor = new ActorDto { Id = 1, Name = "John", Surname = "Doe", SexName = "Male" };

            _mockActorService.Setup(service => service.GetByIdAsync(1, default))
                .ReturnsAsync(actor);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedActor = Assert.IsType<ActorDto>(okResult.Value);
            Assert.Equal(1, returnedActor.Id);
            Assert.Equal("John", returnedActor.Name);
            Assert.Equal("Doe", returnedActor.Surname);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockActorService.Setup(service => service.GetByIdAsync(999, default))
                .ReturnsAsync((ActorDto)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_WithValidDto_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var createDto = new ActorCreateDto { Name = "New Actor", Surname = "New Surname", IdSex = 1 };
            var createdActor = new ActorDto { Id = 1, Name = "New Actor", Surname = "New Surname", IdSex = 1, SexName = "Male" };

            _mockActorService.Setup(service => service.CreateAsync(createDto, default))
                .ReturnsAsync(createdActor);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(ActorsController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);

            var returnedActor = Assert.IsType<ActorDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnedActor.Id);
            Assert.Equal("New Actor", returnedActor.Name);
            Assert.Equal("New Surname", returnedActor.Surname);
        }

        [Fact]
        public async Task Update_WithValidIdAndDto_ShouldReturnNoContent()
        {
            // Arrange
            var updateDto = new ActorUpdateDto { Name = "Updated Actor", Surname = "Updated Surname", IdSex = 2, IsActive = true };

            _mockActorService.Setup(service => service.UpdateAsync(1, updateDto, default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockActorService.Verify(service => service.UpdateAsync(1, updateDto, default), Times.Once);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _mockActorService.Setup(service => service.DeleteAsync(1, default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockActorService.Verify(service => service.DeleteAsync(1, default), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistentId_ShouldStillReturnNoContent()
        {
            // Arrange
            _mockActorService.Setup(service => service.DeleteAsync(999, default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(999);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_WhenServiceThrowsException_ShouldPropagateException()
        {
            // Arrange
            _mockActorService.Setup(service => service.DeleteAsync(1, default))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Delete(1));
        }

        [Fact]
        public async Task Update_WhenServiceThrowsKeyNotFoundException_ShouldPropagateException()
        {
            // Arrange
            var updateDto = new ActorUpdateDto { Name = "Updated Actor", Surname = "Updated Surname", IdSex = 1, IsActive = true };

            _mockActorService.Setup(service => service.UpdateAsync(999, updateDto, default))
                .ThrowsAsync(new KeyNotFoundException("Actor not found"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Update(999, updateDto));
        }
    }
}