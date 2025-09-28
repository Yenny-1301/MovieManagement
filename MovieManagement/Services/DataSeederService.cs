using MovieManagement.AppDataContext;
using MovieManagement.Entities;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services
{
    public class DataSeederService : IDataSeederService
    {
        private readonly ApplicationDataContext _context;
        private readonly ISwapiService _swapiService;

        public DataSeederService(ApplicationDataContext context, ISwapiService swapiService)
        {
            _context = context;
            _swapiService = swapiService;
        }
        public async Task SeedMoviesAsync()
        {
            if (_context.Movies.Any()) return;

            var films = await _swapiService.GetFilmsAsync();

            foreach (var f in films) {
                if(!_context.Movies.Any(m => m.Title == f.title))
                {
                    var movie = new Movie
                    {
                        Id = Guid.NewGuid(),
                        Title = f.title,
                        Director = f.director,
                        ReleaseDate = DateTime.Parse(f.release_date),
                        Producer = f.producer,
                        CreatedAt = DateTime.Now
                    };

                    _context.Movies.Add(movie);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
