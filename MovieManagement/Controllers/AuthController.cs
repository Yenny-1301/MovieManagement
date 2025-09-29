using Microsoft.AspNetCore.Mvc;
using MovieManagement.DTOs.Requests;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtServices _jwtServices;
        private readonly IUsersServices _usersServices;
        private readonly IRoleServices _roleServices;

        public AuthController(IJwtServices jwtServices, IUsersServices usersServices, IRoleServices roleServices)
        {
            _jwtServices = jwtServices;
            _usersServices = usersServices;
            _roleServices = roleServices;
        }

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _usersServices.CreateUserAsync(request);
                return Ok(new { message = "User created" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error ocurred while processing the sign in", error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _usersServices.AuthenticateAsync(request.Email, request.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var role = await _roleServices.GetByIdAsync(user.RoleId);
            user.Role = role;

            var token = _jwtServices.GenerateToken(user);
            return Ok(new { message = "Token successfully created", token = token });

        }
    }
}
