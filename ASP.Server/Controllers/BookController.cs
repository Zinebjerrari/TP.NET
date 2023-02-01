using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using ASP.Server.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Linq;
using YamlDotNet.Core.Tokens;

namespace ASP.Server.Controllers
{
    public class CreateBookModel
    {


        [Required]
        public String Name { get; set; }

        // Ajouter ici tous les champ que l'utilisateur devra remplir pour ajouter un livre
        [Required]

        public string Content { get; set; }
        [Required]

        public double Price { get; set; }

        [Required]

        public string Author { get; set; }



        // Liste des genres séléctionné par l'utilisateur
        public List<int> Genres { get; set; }
        public List<Genre> GenresList { get; set; }

        // Liste des genres a afficher à l'utilisateur
        public IEnumerable<Genre> AllGenres { get; init;  }
    }

    public class ModifyBookModel

    {
        public int Id { get; set; }


        [Required]
        public String Name { get; set; }

        // Ajouter ici tous les champ que l'utilisateur devra remplir pour ajouter un livre
        [Required]

        public string Content { get; set; }
        [Required]

        public double Price { get; set; }

        [Required]

        public string Author { get; set; }



        // Liste des genres séléctionné par l'utilisateur
        public List<int> Genres { get; set; }
        public List<Genre> GenresList { get; set; }

        // Liste des genres a afficher à l'utilisateur
        public IEnumerable<Genre> AllGenres { get; init; }
    }

    public class BookController : Controller
    {
        private readonly LibraryDbContext libraryDbContext;

        public BookController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }

        public ActionResult<IEnumerable<Book>> List()
        {
            // récupérer les livres dans la base de donées pour qu'elle puisse être affiché
             var ListBooks = libraryDbContext.Books.Include(book => book.Genres).OrderBy(book => book.Id).ToList();
            return View(ListBooks);
        }

        public ActionResult<CreateBookModel> Create(CreateBookModel book)
        {
            // Le IsValid est True uniquement si tous les champs de CreateBookModel marqués Required sont remplis
            if (ModelState.IsValid)
            {

                // Il faut intéroger la base pour récupérer l'ensemble des objets genre qui correspond aux id dans CreateBookModel.Genres
                var genres = libraryDbContext.Genre.Where(genre => book.Genres.Contains(genre.Id)).ToList();
                // Completer la création du livre avec toute les information nécéssaire que vous aurez ajoutez, et metter la liste des gener récupéré de la base aussi
                libraryDbContext.Add(new Book()
                {
                    Name = book.Name,
                    Author = book.Author,
                    Price = book.Price,
                    Content = book.Content,
                    Genres = genres
                });
                libraryDbContext.SaveChanges();
                 return RedirectToAction("List");
            }

            // Il faut interoger la base pour récupérer tous les genres, pour que l'utilisateur puisse les slécétionné
            return View(new CreateBookModel() { AllGenres = libraryDbContext.Genre.ToList() });
        }

        public ActionResult<IEnumerable<Book>> Delete(int id)
        {
            var book = libraryDbContext.Books.Single(book => book.Id == id);
            libraryDbContext.Books.Remove(book);
            libraryDbContext.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult<ModifyBookModel> Modify(ModifyBookModel book, long idToModify)
        {
            var bookToModify = libraryDbContext.Books.Include(book => book.Genres).Single(book => book.Id == idToModify);            // Le IsValid est True uniquement si tous les champs de CreateBookModel marqués Required sont remplis

            if (ModelState.IsValid)
            {
                bookToModify.Name = book.Name;
                bookToModify.Author = book.Author;
                bookToModify.Price = book.Price;
                bookToModify.Content = book.Content;

                List<Genre> genres = libraryDbContext.Genre.Where(genre => book.Genres.Contains(genre.Id)).ToList();
                bookToModify.Genres = genres;
                 
                libraryDbContext.SaveChanges();

                return RedirectToAction("List", "Book");

            };

            return View(new ModifyBookModel()
                {
                    Id = bookToModify.Id,
                    Name = bookToModify.Name,
                    GenresList = bookToModify.Genres,
                    Author = bookToModify.Author,
                    Price = bookToModify.Price,
                    Content = bookToModify.Content,
                    AllGenres = libraryDbContext.Genre.ToList()
                });
        }
    }
}

   
