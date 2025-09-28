using MovieManagement.DTOs.Responses;

namespace MovieManagement.Services.Interfaces
{
    public interface ISwapiService
    {
        Task<List<SwapiFilmResponse>> GetFilmsAsync();
    }
}
