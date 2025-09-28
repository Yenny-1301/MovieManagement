using Xunit;
using Moq;
using MovieManagement.Controllers;
using MovieManagement.Services.Interfaces;
using MovieManagement.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Entities;

namespace MovieManagement.Tests.Controllers
{
    public class MovieControllerTests
    {
        private readonly Mock<IMovieServices> _movieServicesMock;
        private readonly MovieController _controller;

        public MovieControllerTests()
        {
            _movieServicesMock = new Mock<IMovieServices>();
            _controller = new MovieController(_movieServicesMock.Object);
        }

        [Fact]
        public async Task CreateMovieAsync_ReturnsOk_WhenModelIsValid()
        {
            var request = new CreateMovieRequest
            {
                Title = "Test",
                Director = "Director",
                ReleaseDate = DateTime.Now,
                Producer = "Producer"
            };

            var result = await _controller.CreateMovieAsync(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateMovieAsync_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _controller.ModelState.AddModelError("Title", "Required");
            var request = new CreateMovieRequest();

            var result = await _controller.CreateMovieAsync(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateMovieAsync_ReturnsServerError_OnException()
        {
            var request = new CreateMovieRequest
            {
                Title = "Test",
                Director = "Director",
                ReleaseDate = DateTime.Now,
                Producer = "Producer"
            };
            _movieServicesMock.Setup(s => s.CreateMovieAsync(It.IsAny<CreateMovieRequest>()))
                .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.CreateMovieAsync(request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOk_WithMovies()
        {
            var movies = new List<Movie> { new Movie { Id = Guid.NewGuid(), Title = "A", Director = "B", Producer = "C" } };
            _movieServicesMock.Setup(s => s.GetAllAsync()).ReturnsAsync(movies);

            var result = await _controller.GetAllAsync();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOk_WithNoMovies()
        {
            _movieServicesMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<Movie>());

            var result = await _controller.GetAllAsync();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsServerError_OnException()
        {
            _movieServicesMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("DB error"));

            var result = await _controller.GetAllAsync();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetMovieByIdAsync_ReturnsOk_WhenFound()
        {
            var id = Guid.NewGuid();
            var movie = new Movie { Id = id, Title = "A", Director = "B", Producer = "C" };
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(movie);

            var result = await _controller.GetMovieByIdAsync(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetMovieByIdAsync_ReturnsNotFound_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Movie)null);

            var result = await _controller.GetMovieByIdAsync(id);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetMovieByIdAsync_ReturnsServerError_OnException()
        {
            var id = Guid.NewGuid();
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new Exception("DB error"));

            var result = await _controller.GetMovieByIdAsync(id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task UpdateMovieAsync_ReturnsOk_WhenUpdated()
        {
            var id = Guid.NewGuid();
            var request = new UpdateMovieRequest { Title = "T", Director = "D", Producer = "P", ReleaseDate = DateTime.Now };
            var movie = new Movie { Id = id, Title = "A", Director = "B", Producer = "C" };
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(movie);

            var result = await _controller.UpdateMovieAsync(id, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateMovieAsync_ReturnsBadRequest_WhenModelIsInvalid()
        {
            _controller.ModelState.AddModelError("Title", "Required");
            var id = Guid.NewGuid();
            var request = new UpdateMovieRequest();

            var result = await _controller.UpdateMovieAsync(id, request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateMovieAsync_ReturnsNotFound_WhenMovieNotFound()
        {
            var id = Guid.NewGuid();
            var request = new UpdateMovieRequest();
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((Movie)null);

            var result = await _controller.UpdateMovieAsync(id, request);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateMovieAsync_ReturnsServerError_OnException()
        {
            var id = Guid.NewGuid();
            var request = new UpdateMovieRequest();
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new Exception("DB error"));

            var result = await _controller.UpdateMovieAsync(id, request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteMovieAsync_ReturnsOk_WhenDeleted()
        {
            var id = Guid.NewGuid();

            var result = await _controller.DeleteMovieAsync(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task DeleteMovieAsync_ReturnsServerError_OnException()
        {
            var id = Guid.NewGuid();
            _movieServicesMock.Setup(s => s.DeleteMovieAsync(id)).ThrowsAsync(new Exception("DB error"));

            var result = await _controller.DeleteMovieAsync(id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}