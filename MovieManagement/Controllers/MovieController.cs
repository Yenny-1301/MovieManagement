using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieManagement.DTOs.Requests;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieServices _movieServices;

        public MovieController(IMovieServices movieServices)
        {
            _movieServices = movieServices;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateMovieAsync(CreateMovieRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _movieServices.CreateMovieAsync(request);
                return Ok(new { message = "Movie item created" });
            }
            catch (Exception ex) 
            { 
                return StatusCode(500, new {message = "An error ocurred while creating the movie item", error = ex.Message});            
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var movie = await _movieServices.GetAllAsync();

                if (movie == null || !movie.Any())
                {
                    return Ok(new { message = "No movie items found" });
                }

                return Ok(new { message = "Successfully returned all movies", data = movie });
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new {message = "An error ocurred while retrieving all movie items", error = ex.Message});
            }
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMovieByIdAsync(Guid id)
        {
            try
            {
                var movie = await _movieServices.GetByIdAsync(id);

                if (movie == null)
                {
                    return NotFound(new { message = $"No Movie with Id {id} found." });
                }

                return Ok(new { message = $"Successfully retrieved Movie with Id {id}.", data = movie });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while retrieving the Movie with Id {id}.", error = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovieAsync(Guid id, UpdateMovieRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var movie = await _movieServices.GetByIdAsync(id);
                if (movie == null)
                {
                    return NotFound(new {message = $"Movie with id {id} not found" });
                }

                await _movieServices.UpdateMovieAsync(id,request);

                return Ok(new { message = $"Movie with id {id} successfully updated" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while updating Movie with id {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovieAsync(Guid id)
        {
            try
            {
                await _movieServices.DeleteMovieAsync(id);
                return Ok(new { message = $"Movie with id {id} successfully deleted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred while deleting Movie with id {id}", error = ex.Message });
            }
        }
    }
}
