using MovieManagement.DTOs.Requests;
using MovieManagement.DTOs.Responses;
using MovieManagement.Entities;

namespace MovieManagement.Services.Interfaces
{
    public interface IMovieServices
    {
        Task<IEnumerable<MovieResponse>> GetAllAsync();
        Task <MovieResponse> GetByIdAsync(Guid id);
        Task CreateMovieAsync(CreateMovieRequest request);
        Task UpdateMovieAsync(Guid id, UpdateMovieRequest request);
        Task DeleteMovieAsync(Guid id);
    }
}
