using System.ComponentModel.DataAnnotations;

namespace MovieManagement.DTOs.Requests
{
    public class UpdateUserRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Guid RoleId { get; set; }
    }
}
