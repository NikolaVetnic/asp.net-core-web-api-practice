using Movies.Api.Model;

namespace Movies.Api.Data;

public class MoviesInitialData
{
    public static void SeedAsync(MoviesDbContext moviesDbContext)
    {
        if (moviesDbContext.Movies.Any()) 
            return;
        
        var movies = new List<Movie>
        {
            new()
            {
                Id = 1,
                Genre = "Drama",
                Title = "The Shawshank Redemption",
                Rating = "9.3",
                ImageUrl = "images/src",
                ReleaseDate = new DateTime(1994, 5, 5),
                Owner = "alice"
            },
            new()
            {
                Id = 2,
                Genre = "Crime",
                Title = "The Godfather",
                Rating = "9.2",
                ImageUrl = "images/src",
                ReleaseDate = new DateTime(1972, 5, 5),
                Owner = "alice"
            },
            new()
            {
                Id = 3,
                Genre = "Action",
                Title = "The Dark Knight",
                Rating = "9.1",
                ImageUrl = "images/src",
                ReleaseDate = new DateTime(2008, 5, 5),
                Owner = "bob"
            },
            new()
            {
                Id = 4,
                Genre = "Crime",
                Title = "12 Angry Men",
                Rating = "8.9",
                ImageUrl = "images/src",
                ReleaseDate = new DateTime(1957, 5, 5),
                Owner = "bob"
            },
            new()
            {
                Id = 5,
                Genre = "Biography",
                Title = "Schindler's List",
                Rating = "8.9",
                ImageUrl = "images/src",
                ReleaseDate = new DateTime(1993, 5, 5),
                Owner = "alice"
            },
            new()
            {
                Id = 6,
                Genre = "Drama",
                Title = "Pulp Fiction",
                Rating = "8.9",
                ImageUrl = "images/src",
                ReleaseDate = new DateTime(1994, 5, 5),
                Owner = "alice"
            },
            new()
            {
                Id = 7,
                Genre = "Drama",
                Title = "Fight Club",
                Rating = "8.8",
                ImageUrl = "images/src",
                ReleaseDate = new DateTime(1999, 5, 5),
                Owner = "bob"
            },
            new()
            {
                Id = 8,
                Genre = "Romance",
                Title = "Forrest Gump",
                Rating = "8.8",
                ImageUrl = "images/src",
                ReleaseDate = new DateTime(1994, 5, 5),
                Owner = "bob"
            }
        };
        
        moviesDbContext.Movies.AddRange(movies);
        moviesDbContext.SaveChanges();
    }
}
