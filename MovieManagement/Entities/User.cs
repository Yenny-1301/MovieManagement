using System.ComponentModel.DataAnnotations;

namespace MovieManagement.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Email { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid RoleId { get; set; }
        public Role Role {  get; set; }
    }
}
