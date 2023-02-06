using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.Server.Database;
using Microsoft.Extensions.DependencyModel;
namespace ASP.Server.Api
{
    [Route("/api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext libraryDbContext;
        public BookController(LibraryDbContext libraryDbContext)
        {
            this.libraryDbContext = libraryDbContext;
        }
        // Aide:
        // Pour récupéré un objet d'une table :
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.First()
        // Pour récupéré des objets d'une table :
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.ToList()
        // Pour faire une requète avec filtre:
        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.Skip().<Selecteurs>        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.Take().<Selecteurs>        //   - libraryDbContext.MyObjectCollection.<Selecteurs>.Where(x => x == y).<Selecteurs>        // Pour récupérer une 2nd table depuis la base:
        //   - .Include(x => x.yyyyy)
        //     ou yyyyy est la propriété liant a une autre table a récupéré
        //
        // Exemple:
        //   - Ex: libraryDbContext.MyObjectCollection.Include(x => x.yyyyy).Where(x => x.yyyyyy.Contains(z)).Skip(i).Take(j).ToList()
        // - GetBook
        //   - Entrée: Id du livre
        //   - Sortie: Object livre entier
        [HttpGet("GetBook/{Id}")] 
        public ActionResult<Book> GetBook(int Id)
        {
            var test = libraryDbContext.Books.Include(g => g.Genres).Where(g => g.Id == Id).FirstOrDefault();

            if (test == null)
            {
                return NotFound();
            }

            return Ok(test);
        }
        // - GetGenres
        //   - Entrée: Rien
        //   - Sortie: Liste des genres
        [HttpGet("GetGenres")]
        public ActionResult<List<Genre>> GetGenres()
        {
            return libraryDbContext.Genre.ToList();
        }
        // Methode a ajouter : 
        // - GetBooks
        //   - Entrée: Optionel -> Liste d'Id de genre, limit -> defaut à 10, offset -> défaut à 0
        //     Le but de limit et offset est dé créer un pagination pour ne pas retourner la BDD en entier a chaque appel
        //   - Sortie: Liste d'object contenant uniquement: Auteur, Genres, Titre, Id, Prix
        //     la liste retourner doit être compsé des élément entre <offset> et <offset + limit>-
        //     Dans [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20] si offset=8 et limit=5, les élément retourner seront : 8, 9, 10, 11, 12
        // Je vous montre comment faire la 1er, a vous de la compléter et de faire les autres !

        [HttpGet("GetBooks/{genres?}/{offset}/{limit}")]
        public ActionResult<IEnumerable<BookWithoutContent>> GetBooks(string genres = null, int offset = 0, int limit = 10)
        {
            if (genres != "null")
            {
                var list = genres.Split(",").Select(x => int.Parse(x)).ToList();
                return libraryDbContext.Books.Include(g => g.Genres).Where(g => list.Contains(g.Id)).Skip(offset).Take(offset + limit).Select(book => new BookWithoutContent() { Genres = book.Genres, Id = book.Id, Name = book.Name, Price = book.Price, Author = book.Author }).ToList();
            }else
            {
                return libraryDbContext.Books.Include(b => b.Genres).Skip(offset).Take(offset+limit).Select(book => new BookWithoutContent() { Genres = book.Genres, Id = book.Id, Name = book.Name, Price = book.Price, Author = book.Author }).ToList(); 
            }
           
        }
    }
}