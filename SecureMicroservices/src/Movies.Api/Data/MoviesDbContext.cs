using Microsoft.EntityFrameworkCore;
using Movies.Api.Model;

namespace Movies.Api.Data;

public class MoviesDbContext(DbContextOptions<MoviesDbContext> options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; }
}
