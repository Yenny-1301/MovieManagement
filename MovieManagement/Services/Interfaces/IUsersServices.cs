using MovieManagement.DTOs.Requests;
using MovieManagement.DTOs.Responses;
using MovieManagement.Entities;

namespace MovieManagement.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse> GetByIdAsync(Guid id);
        Task CreateUserAsync(CreateUserRequest request);
        Task UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task DeleteUserAsync(Guid id);
        Task<User?> AuthenticateAsync(string email, string password);
    }
}
