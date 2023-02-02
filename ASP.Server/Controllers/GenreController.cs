using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;
using ASP.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ASP.Server.Controllers
{

    public class CreateGenreModel
    {
        [Required]
        public string Name { get; set; }
    }

    public class ModifyGenreModel
    {
        public long Id { get; set; }

        [Required]

        public string Name { get; set; }
    }


    public class GenreController : Controller
    {
        private readonly LibraryDbContext libraryDbContext;

        public GenreController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }

        // A vous de faire comme BookController.List mais pour les genres !
        public ActionResult<IEnumerable<Genre>> List()
        {
            var listGenres = libraryDbContext.Genre.Include(g => g.Books).ToList();
            return View(listGenres);
        }

        public ActionResult<CreateGenreModel> Create(CreateGenreModel genre)
        {
            if (ModelState.IsValid)
            {
                libraryDbContext.Add(new Genre()
                { Name = genre.Name });
                libraryDbContext.SaveChanges();
                return RedirectToAction("List");

            };

            return View(new CreateGenreModel() { });

        }

        public ActionResult<ModifyGenreModel> Modify(ModifyGenreModel genre, long idToModify)
        {
            var genreToModify = libraryDbContext.Genre.Single(genre => genre.Id == idToModify);

            if (ModelState.IsValid) {

                genreToModify.Name = genre.Name;
                libraryDbContext.SaveChanges();
                return RedirectToAction("List");

            };

                return View(new ModifyGenreModel() 
                { Id = genreToModify.Id, Name = genreToModify.Name });
            
        }

        public ActionResult<IEnumerable<Genre>> Delete(long id)
        {
            var genre = libraryDbContext.Genre.Single(genre => genre.Id == id);
            libraryDbContext.Genre.Remove(genre);
            libraryDbContext.SaveChanges();
            return RedirectToAction("List", "Genre");
        }

    }
}
