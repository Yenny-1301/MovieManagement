using System.ComponentModel.DataAnnotations;

namespace MovieManagement.DTOs.Requests
{
    public class CreateMovieRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public string Producer { get; set; }
    }
}
