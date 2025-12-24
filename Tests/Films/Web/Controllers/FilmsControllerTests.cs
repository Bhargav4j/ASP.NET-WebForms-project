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
    public class FilmsControllerTests
    {
        private readonly Mock<IFilmService> _mockFilmService;
        private readonly FilmsController _controller;

        public FilmsControllerTests()
        {
            _mockFilmService = new Mock<IFilmService>();
            _controller = new FilmsController(_mockFilmService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithListOfFilms()
        {
            // Arrange
            var films = new List<FilmDto>
            {
                new FilmDto { Id = 1, Name = "Film 1", Description = "Description 1" },
                new FilmDto { Id = 2, Name = "Film 2", Description = "Description 2" }
            };

            _mockFilmService.Setup(service => service.GetAllAsync(default))
                .ReturnsAsync(films);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFilms = Assert.IsAssignableFrom<IEnumerable<FilmDto>>(okResult.Value);
            Assert.Equal(2, returnedFilms.Count());
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnOkResult_WithFilm()
        {
            // Arrange
            var film = new FilmDto { Id = 1, Name = "Test Film", Description = "Test Description" };

            _mockFilmService.Setup(service => service.GetByIdAsync(1, default))
                .ReturnsAsync(film);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedFilm = Assert.IsType<FilmDto>(okResult.Value);
            Assert.Equal(1, returnedFilm.Id);
            Assert.Equal("Test Film", returnedFilm.Name);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            _mockFilmService.Setup(service => service.GetByIdAsync(999, default))
                .ReturnsAsync((FilmDto)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_WithValidDto_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var createDto = new FilmCreateDto { Name = "New Film", Description = "New Description" };
            var createdFilm = new FilmDto { Id = 1, Name = "New Film", Description = "New Description" };

            _mockFilmService.Setup(service => service.CreateAsync(createDto, default))
                .ReturnsAsync(createdFilm);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(FilmsController.GetById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);

            var returnedFilm = Assert.IsType<FilmDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnedFilm.Id);
            Assert.Equal("New Film", returnedFilm.Name);
        }

        [Fact]
        public async Task Update_WithValidIdAndDto_ShouldReturnNoContent()
        {
            // Arrange
            var updateDto = new FilmUpdateDto { Name = "Updated Film", Description = "Updated Description", IsActive = true };

            _mockFilmService.Setup(service => service.UpdateAsync(1, updateDto, default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockFilmService.Verify(service => service.UpdateAsync(1, updateDto, default), Times.Once);
        }

        [Fact]
        public async Task Delete_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            _mockFilmService.Setup(service => service.DeleteAsync(1, default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockFilmService.Verify(service => service.DeleteAsync(1, default), Times.Once);
        }

        [Fact]
        public async Task Delete_WithNonExistentId_ShouldStillReturnNoContent()
        {
            // Arrange
            _mockFilmService.Setup(service => service.DeleteAsync(999, default))
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
            _mockFilmService.Setup(service => service.DeleteAsync(1, default))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Delete(1));
        }

        [Fact]
        public async Task Update_WhenServiceThrowsKeyNotFoundException_ShouldPropagateException()
        {
            // Arrange
            var updateDto = new FilmUpdateDto { Name = "Updated Film", Description = "Updated Description", IsActive = true };

            _mockFilmService.Setup(service => service.UpdateAsync(999, updateDto, default))
                .ThrowsAsync(new KeyNotFoundException("Film not found"));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _controller.Update(999, updateDto));
        }
    }
}