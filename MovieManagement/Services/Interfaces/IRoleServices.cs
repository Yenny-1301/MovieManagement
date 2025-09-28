using Microsoft.AspNetCore.Mvc;
using MovieManagement.Entities;

namespace MovieManagement.Services.Interfaces
{
    public interface IRoleServices
    {
        public Task<IEnumerable<Role>> GetAllAsync();
        public Task<Role> GetByIdAsync(Guid id);
    }
}
