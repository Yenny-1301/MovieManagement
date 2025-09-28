using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieManagement.AppDataContext;
using MovieManagement.DTOs.Requests;
using MovieManagement.Entities;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly ApplicationDataContext _context;
        private readonly ILogger<MovieServices> _logger;
        private readonly IMapper _mapper;

        public MovieServices(ApplicationDataContext context,ILogger<MovieServices> logger,IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task CreateMovieAsync(CreateMovieRequest request)
        {
            try
            {
                var movie = _mapper.Map<Movie>(request);
                
                movie.CreatedAt = DateTime.UtcNow;
                movie.UpdatedAt = DateTime.UtcNow;
                
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) {
                var message = "An error ocurred while creating the Movie item";
                _logger.LogError(ex, message);
                throw new Exception(message);
            }
        }

        public async Task DeleteMovieAsync(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                throw new KeyNotFoundException($"No movie found with Id {id} found");
            }
            else
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            var movies = await _context.Movies.ToListAsync();
            if (movies == null)
            {
                throw new Exception("No Movies found");
            }

            return movies;
        }

        public async Task<Movie> GetByIdAsync(Guid id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                throw new KeyNotFoundException($"No movie found with Id {id} found");
            }
            return movie;
        }

        public async Task UpdateMovieAsync(Guid id, UpdateMovieRequest request)
        {
            try
            {
                var movie = await _context.Movies.FindAsync(id);

                if (movie == null)
                {
                    throw new Exception($"Movie item with id {id} not found");
                }

                if (request.Title != null)
                {
                    movie.Title = request.Title;
                }

                if (request.Director != null)
                {
                    movie.Director = request.Director;
                }
                if (request.ReleaseDate != null)
                {
                    movie.ReleaseDate = request.ReleaseDate;
                }
                if (request.Producer != null)
                {
                    movie.Producer = request.Producer;
                }

                movie.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = $"An error occurred while updating the todo item with id {id}.";
                _logger.LogError(ex, message);
                throw;
            }
        }
    }
}
