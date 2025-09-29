using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieManagement.AppDataContext;
using MovieManagement.DTOs.Requests;
using MovieManagement.DTOs.Responses;
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
                if (!IsValidEmail(request.Email))
                    throw new ArgumentException("The email format is not valid.");

                if (!IsStrongPassword(request.Password))
                    throw new ArgumentException("The password must have at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character.");

                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                    throw new InvalidOperationException("A user with this email already exists.");

                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.Role);
                if (role == null)
                    throw new ArgumentException("Invalid role specified.");

                var user = _mapper.Map<User>(request);

                user.Id = Guid.NewGuid();
                user.CreatedAt = DateTime.UtcNow;
                user.UpdatedAt = DateTime.UtcNow;
                user.Password = _jwtServices.HashPassword(request.Password);
                user.RoleId = role.Id;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating user");
                throw;
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

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();

            if (users == null)
            {
                throw new Exception("No users found");
            }

            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"No user found with id {id}");
            }

            return _mapper.Map<UserResponse>(user);
        }

        public async Task UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    throw new Exception($"User with id {id} not found");

                if (!IsValidEmail(request.Email))
                    throw new Exception("The email format is not valid.");

                if (!IsStrongPassword(request.Password))
                    throw new Exception("The password must have at least 8 characters, one uppercase letter, one lowercase letter, one number and one special character.");

                if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                    throw new Exception("A user with this email already exists.");

                if (!await _context.Roles.AnyAsync(r => r.Id == request.RoleId))
                    throw new Exception("Invalid role specified.");

                user.Name = request.Name;
                user.Email = request.Email;
                user.Password = _jwtServices.HashPassword(request.Password);
                user.RoleId = request.RoleId;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message = $"An error occurred while updating the todo item with id {id}.";
                _logger.LogError(ex, message);
                throw;
            }
        }
        private bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
        }

        private bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var hashedPassword = _jwtServices.HashPassword(password);
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);
        }
    }
}
