using MovieManagement.DTOs.Requests;
using MovieManagement.Entities;

namespace MovieManagement.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task CreateUserAsync(CreateUserRequest request);
        Task UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task DeleteUserAsync(Guid id);
    }
}
