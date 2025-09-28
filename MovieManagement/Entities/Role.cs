using System.ComponentModel.DataAnnotations;

namespace MovieManagement.Entities
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
