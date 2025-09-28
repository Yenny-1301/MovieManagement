using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDataSeederService _dataseeder;

        public AdminController(IDataSeederService dataseeder) 
        {
            _dataseeder = dataseeder;
        }

        [HttpPost("seed/movies")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SeedMovies()
        {
            await _dataseeder.SeedMoviesAsync();
            return Ok("Star Wars Movies imported successfully");
        }
    }
}
