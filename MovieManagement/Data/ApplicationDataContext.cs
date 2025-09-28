using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MovieManagement.Config;
using MovieManagement.Entities;

namespace MovieManagement.AppDataContext
{
    public class ApplicationDataContext : DbContext
    {
        private readonly DbSettings _dbsettings;


        public ApplicationDataContext(IOptions<DbSettings> dbSettings)
        {
            _dbsettings = dbSettings.Value;        
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbsettings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .ToTable("Movies");

            modelBuilder.Entity<Role>()
                .ToTable("Roles")
                .HasData( 
                    new Role { Id = Guid.NewGuid(), Name = "User" },
                    new Role { Id = Guid.NewGuid(), Name = "Admin" }
                    );

            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
        }

    }
}
