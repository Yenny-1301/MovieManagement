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
            _controller = new AuthController(_jwtServicesMock.Object, _usersServicesMock.Object, _roleServicesMock.Object);
        }

        [Fact]
        public async Task SignUp_ReturnsOk_WhenModelIsValid()
        {
            var request = new CreateUserRequest
            {
                Name = "Test",
                Email = "test@mail.com",
                Password = "1234",
                RoleId = Guid.NewGuid()
            };

            var result = await _controller.SignUp(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task SignUp_ReturnsBadRequest_WhenModelIsInvalid()
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
                Password = "1234",
                RoleId = Guid.NewGuid()
            };
            _usersServicesMock.Setup(s => s.CreateUserAsync(It.IsAny<CreateUserRequest>()))
                .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.SignUp(request);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            var userId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var password = "1234";
            var hashedPassword = "hashed";
            var user = new User
            {
                Id = userId,
                Name = "Test",
                Email = "test@mail.com",
                Password = hashedPassword,
                RoleId = roleId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            var users = new List<User> { user };
            var role = new Role { Id = roleId, Name = "User" };

            _usersServicesMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);
            _jwtServicesMock.Setup(s => s.HashPassword(password)).Returns(hashedPassword);
            _roleServicesMock.Setup(s => s.GetByIdAsync(roleId)).ReturnsAsync(role);
            _jwtServicesMock.Setup(s => s.GenerateToken(It.IsAny<User>())).Returns("token");

            var request = new LoginRequest { Email = "test@mail.com", Password = password };

            var result = await _controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Contains("token", okResult.Value.ToString());
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Test",
                    Email = "test@mail.com",
                    Password = "hashed",
                    RoleId = Guid.NewGuid(),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            };
            _usersServicesMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);
            _jwtServicesMock.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("otherhash");

            var request = new LoginRequest { Email = "test@mail.com", Password = "wrong" };

            var result = await _controller.Login(request);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
        {
            _usersServicesMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<User>());
            var request = new LoginRequest { Email = "notfound@mail.com", Password = "1234" };

            var result = await _controller.Login(request);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}