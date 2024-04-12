using Microsoft.EntityFrameworkCore;
using MovieAppWebApi.Models;

namespace MovieAppWebApi
{
    public class MovieAppDbContext:DbContext
    {
        public MovieAppDbContext(DbContextOptions<MovieAppDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FilmCategories> FilmCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieAppDbContext).Assembly);
        }
    }
}
