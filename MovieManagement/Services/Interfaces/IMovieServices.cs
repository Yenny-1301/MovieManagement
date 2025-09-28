using MovieManagement.DTOs.Requests;
using MovieManagement.Entities;

namespace MovieManagement.Services.Interfaces
{
    public interface IMovieServices
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task <Movie> GetByIdAsync(Guid id);
        Task CreateMovieAsync(CreateMovieRequest request);
        Task UpdateMovieAsync(Guid id, UpdateMovieRequest request);
        Task DeleteMovieAsync(Guid id);
    }
}
