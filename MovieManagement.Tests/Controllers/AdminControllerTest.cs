using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieManagement.Controllers;
using MovieManagement.Services.Interfaces;
using Xunit;

namespace MovieManagement.Tests.Controllers
{
    public class AdminControllerTests
    {
        private readonly Mock<IDataSeederService> _dataSeederMock;
        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _dataSeederMock = new Mock<IDataSeederService>();
            _controller = new AdminController(_dataSeederMock.Object);
        }

        [Fact]
        public async Task SeedMovies_ReturnsOk_WhenServiceSucceeds()
        {
            _dataSeederMock.Setup(s => s.SeedMoviesAsync()).Returns(Task.CompletedTask);

            var result = await _controller.SeedMovies();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Star Wars Movies imported successfully", okResult.Value);
        }

        [Fact]
        public async Task SeedMovies_ReturnsServerError_WhenServiceThrowsException()
        {
            _dataSeederMock.Setup(s => s.SeedMoviesAsync()).ThrowsAsync(new Exception("DB error"));
            await Assert.ThrowsAsync<Exception>(() => _controller.SeedMovies());
        }
    }
}