using System.ComponentModel.DataAnnotations;

namespace MovieManagement.DTOs.Responses
{
    public class MovieResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Producer { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
