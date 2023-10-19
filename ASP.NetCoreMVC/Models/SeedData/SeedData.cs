using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ASP.NetCoreMVC.Data;
using System;
using System.Linq;

namespace ASP.NetCoreMVC.Models.SeedData
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcMovieContext>>())) 
            { 
                // Look for any movies.
                if (context.Movie.Any())
                {
                    return; //DB has been seeded
                }

                context.Movie.AddRange(

                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Price = 7.99M,
                        Genre = "Romantic Comedy",
                        Rating = "R"
                    },
                    new Movie
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Price = 8.99M,
                        Genre = "Comedy",
                        Rating = "R"
                    },
                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Price = 9.99M,
                        Genre = "Comedy",
                        Rating = "R"

                    },
                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Price = 3.99M,
                        Genre = "Western",
                        Rating = "R"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
