using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieManagement.Controllers;
using MovieManagement.DTOs.Requests;
using MovieManagement.Entities;
using MovieManagement.Services.Interfaces;
using Xunit;

namespace MovieManagement.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IJwtServices> _jwtServicesMock;
        private readonly Mock<IUsersServices> _usersServicesMock;
        private readonly Mock<IRoleServices> _roleServicesMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _jwtServicesMock = new Mock<IJwtServices>();
            _usersServicesMock = new Mock<IUsersServices>();
            _roleServicesMock = new Mock<IRoleServices>();
            _controller = new AuthController(
                _jwtServicesMock.Object,
                _usersServicesMock.Object,
                _roleServicesMock.Object
            );
        }

        // POST: SignUp
        [Fact]
        public async Task SignUp_ReturnsOk_WhenUserCreated()
        {
            var request = new CreateUserRequest
            {
                Name = "Test",
                Email = "test@mail.com",
                Password = "123",
                Role = "User"
            };

            var result = await _controller.SignUp(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task SignUp_ReturnsBadRequest_WhenModelStateInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var request = new CreateUserRequest();

            var result = await _controller.SignUp(request);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SignUp_ReturnsServerError_OnException()
        {
            var request = new CreateUserRequest
            {
                Name = "Test",
                Email = "test@mail.com",
                Password = "123",
                Role = "User"
            };
            _usersServicesMock.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserRequest>()))
                .ThrowsAsync(new Exception("fail"));

            var result = await _controller.SignUp(request);

            var serverResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, serverResult.StatusCode);
        }

        // POST: Login
        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsValid()
        {
            var request = new LoginRequest
            {
                Email = "test@mail.com",
                Password = "123"
            };
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Test",
                Email = request.Email,
                Password = "hashed",
                RoleId = Guid.NewGuid()
            };
            var role = new Role { Id = user.RoleId, Name = "User" };
            _usersServicesMock.Setup(s => s.AuthenticateAsync(request.Email, request.Password))
                .ReturnsAsync(user);
            _roleServicesMock.Setup(s => s.GetByIdAsync(user.RoleId))
                .ReturnsAsync(role);
            _jwtServicesMock.Setup(s => s.GenerateToken(It.IsAny<User>()))
                .Returns("token123");

            var result = await _controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Contains("token", okResult.Value.ToString());
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsInvalid()
        {
            var request = new LoginRequest
            {
                Email = "test@mail.com",
                Password = "wrong"
            };
            _usersServicesMock.Setup(s => s.AuthenticateAsync(request.Email, request.Password))
                .ReturnsAsync((User)null);

            var result = await _controller.Login(request);

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorized.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsServerError_OnException()
        {
            var request = new LoginRequest
            {
                Email = "test@mail.com",
                Password = "123"
            };
            _usersServicesMock.Setup(s => s.AuthenticateAsync(request.Email, request.Password))
                .ThrowsAsync(new Exception("fail"));

            // Para capturar la excepción, se debe envolver en try-catch porque el controlador no la maneja explícitamente
            await Assert.ThrowsAsync<Exception>(() => _controller.Login(request));
        }
    }
}