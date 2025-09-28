using Microsoft.EntityFrameworkCore;
using MovieManagement.AppDataContext;
using MovieManagement.Entities;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services
{
    public class RoleServices : IRoleServices
    {
        private readonly ApplicationDataContext _context;

        public RoleServices(ApplicationDataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            if (roles == null)
            {
                throw new Exception("No Roles found");
            }

            return roles; 
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                throw new KeyNotFoundException($"No role found with id {id}");
            }

            return role;
        }
    }
}
