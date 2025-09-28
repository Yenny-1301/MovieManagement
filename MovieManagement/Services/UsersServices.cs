using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieManagement.AppDataContext;
using MovieManagement.DTOs.Requests;
using MovieManagement.Entities;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly ApplicationDataContext _context;
        private readonly ILogger<UsersServices> _logger;
        private readonly IMapper _mapper;
        private readonly IJwtServices _jwtServices;

        public UsersServices(ApplicationDataContext context,ILogger<UsersServices> logger,IMapper mapper, IJwtServices jwtServices)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _jwtServices = jwtServices;
        }
        public async Task CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    throw new Exception("A user with this email already exists.");
                }

                if (!await _context.Roles.AnyAsync(r => r.Id == request.RoleId))
                {
                    throw new Exception("Invalid role specified.");
                }

                var user = _mapper.Map<User>(request);

                user.Id = Guid.NewGuid();
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                user.Password = _jwtServices.HashPassword(request.Password);

                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId);
                user.RoleId = role.Id;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = "An error ocurred while creating the User";
                _logger.LogError(ex, message);
                throw new Exception(message);
            }
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = _context.Users.Find(id);

            if (user != null) {
                _context.Users.Remove(user);
                await  _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"No user found with id {id}");
            }

        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();

            if (users == null)
            {
                throw new Exception("No users found");
            }

            return users;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"No user found with id {id}");
            }

            return user;
        }

        public async Task UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    throw new Exception($"User with id {id} not found");
                }

                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                {
                    throw new Exception("A user with this email already exists.");
                }

                if (!await _context.Roles.AnyAsync(r => r.Id == request.RoleId))
                {
                    throw new Exception("Invalid role specified.");
                }


                if (request.Name != null)
                {
                    user.Name = request.Name;
                }
                if (request.Email != null)
                {
                    user.Email = request.Email;
                }
                if (request.Password != null)
                {
                    user.Password =  request.Password;
                }

                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId);
                user.RoleId = role.Id;
                user.Role = role;

                user.UpdatedAt = DateTime.Now;
                user.RoleId = request.RoleId;

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
