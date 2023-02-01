using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP.Server.Database
{
    public class DbInitializer
    {
        public static void Initialize(LibraryDbContext bookDbContext)
        {

            if (bookDbContext.Books.Any())
                return;

            Genre SF, Classic, Romance, Thriller;
            bookDbContext.Genre.AddRange(
                SF = new Genre() { Name = "SF"},
                Classic = new Genre() { Name = "Classic" },
                Romance = new Genre() { Name = "Romance" },
                Thriller = new Genre() { Name = "Thriller" }
            );
            bookDbContext.SaveChanges();

            // Une fois les moèles complété Vous pouvez faire directement
            // new Book() { Author = "xxx", Name = "yyy", Price = n.nnf, Content = "ccc", Genres = new() { Romance, Thriller } }
            bookDbContext.Books.AddRange(
                new Book()
                {

                    Name = "Titre1",
                    Author = "Mehdi",
                    Content = "test,hubioif25@@@    !!!!    ",
                    Price = 15.51,
                    Genres = new() { SF}
                },

                new Book()
                {

                    Name = "Titre2",
                    Author = "Mehdi2",
                    Content = "test,hubioif25@@@    !!!!    ",
                    Price = 9526,
                    Genres = new() { Classic }

                },
                new Book()
                {

                    Name = "Titre3",
                    Author = "Mehdi3",
                    Content = "test,hubioif25@@@    !!!!    ",
                    Price = 986120684126,
                    Genres = new() {  Romance }

                })


         ;
            // Vous pouvez initialiser la BDD ici

            bookDbContext.SaveChanges();
        }
    }
}