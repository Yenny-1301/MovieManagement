using MovieManagement.Entities;

namespace MovieManagement.Services.Interfaces
{
    public interface IJwtServices
    {
        public string HashPassword(string password);
        public string GenerateToken(User user);
    }
}
