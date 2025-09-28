using System.ComponentModel.DataAnnotations;

namespace MovieManagement.Entities
{
    public class Movie
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Director { get; set; }
        public DateTime? ReleaseDate { get; set; }
        [Required]
        public string Producer {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
