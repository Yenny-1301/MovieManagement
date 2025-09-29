using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieManagement.Controllers;
using MovieManagement.DTOs.Requests;
using MovieManagement.DTOs.Responses;
using MovieManagement.Services.Interfaces;
using Xunit;

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

        // POST: CreateMovieAsync
        [Fact]
        public async Task CreateMovieAsync_ReturnsOk_WhenMovieCreated()
        {
            var request = new CreateMovieRequest
            {
                Title = "Test",
                Director = "Dir",
                ReleaseDate = DateTime.UtcNow,
                Producer = "Prod"
            };

            var result = await _controller.CreateMovieAsync(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task CreateMovieAsync_ReturnsBadRequest_WhenModelStateInvalid()
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
                Director = "Dir",
                ReleaseDate = DateTime.UtcNow,
                Producer = "Prod"
            };
            _movieServicesMock.Setup(s => s.CreateMovieAsync(It.IsAny<CreateMovieRequest>()))
                .ThrowsAsync(new Exception("fail"));

            var result = await _controller.CreateMovieAsync(request);

            var serverResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverResult.StatusCode);
        }

        // GET: GetAllAsync
        [Fact]
        public async Task GetAllAsync_ReturnsOk_WithMovies()
        {
            var movies = new List<MovieResponse> { new MovieResponse { Id = Guid.NewGuid(), Title = "A" } };
            _movieServicesMock.Setup(s => s.GetAllAsync()).ReturnsAsync(movies);

            var result = await _controller.GetAllAsync();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOk_WhenNoMovies()
        {
            _movieServicesMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<MovieResponse>());

            var result = await _controller.GetAllAsync();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsServerError_OnException()
        {
            _movieServicesMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("fail"));

            var result = await _controller.GetAllAsync();

            var serverResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverResult.StatusCode);
        }

        // GET: GetMovieByIdAsync
        [Fact]
        public async Task GetMovieByIdAsync_ReturnsOk_WhenMovieFound()
        {
            var id = Guid.NewGuid();
            var movie = new MovieResponse { Id = id, Title = "A" };
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(movie);

            var result = await _controller.GetMovieByIdAsync(id);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetMovieByIdAsync_ReturnsNotFound_WhenMovieNotFound()
        {
            var id = Guid.NewGuid();
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((MovieResponse)null);

            var result = await _controller.GetMovieByIdAsync(id);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }

        [Fact]
        public async Task GetMovieByIdAsync_ReturnsServerError_OnException()
        {
            var id = Guid.NewGuid();
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.GetMovieByIdAsync(id);

            var serverResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverResult.StatusCode);
        }

        // PUT: UpdateMovieAsync
        [Fact]
        public async Task UpdateMovieAsync_ReturnsOk_WhenMovieUpdated()
        {
            var id = Guid.NewGuid();
            var request = new UpdateMovieRequest { Title = "T" };
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(new MovieResponse { Id = id });

            var result = await _controller.UpdateMovieAsync(id, request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task UpdateMovieAsync_ReturnsNotFound_WhenMovieNotFound()
        {
            var id = Guid.NewGuid();
            var request = new UpdateMovieRequest { Title = "T" };
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((MovieResponse)null);

            var result = await _controller.UpdateMovieAsync(id, request);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFound.StatusCode);
        }

        [Fact]
        public async Task UpdateMovieAsync_ReturnsBadRequest_WhenModelStateInvalid()
        {
            _controller.ModelState.AddModelError("Title", "Required");
            var id = Guid.NewGuid();
            var request = new UpdateMovieRequest();

            var result = await _controller.UpdateMovieAsync(id, request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateMovieAsync_ReturnsServerError_OnException()
        {
            var id = Guid.NewGuid();
            var request = new UpdateMovieRequest { Title = "T" };
            _movieServicesMock.Setup(s => s.GetByIdAsync(id)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.UpdateMovieAsync(id, request);

            var serverResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverResult.StatusCode);
        }

        // DELETE: DeleteMovieAsync
        [Fact]
        public async Task DeleteMovieAsync_ReturnsOk_WhenMovieDeleted()
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
            _movieServicesMock.Setup(s => s.DeleteMovieAsync(id)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.DeleteMovieAsync(id);

            var serverResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverResult.StatusCode);
        }
    }
}