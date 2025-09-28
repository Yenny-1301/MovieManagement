using System.ComponentModel.DataAnnotations;

namespace MovieManagement.DTOs.Requests
{
    public class UpdateMovieRequest
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Producer { get; set; }
    }
}
