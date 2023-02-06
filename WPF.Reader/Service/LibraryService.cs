using MyNamespace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Documents;

namespace WPF.Reader.Service
{
    public class LibraryService
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://localhost:5000")
        };
        public LibraryService()
        {
            UpdateBookList();
            UpdateGenreList();
        }

        // A remplacer avec vos propre données !!!!!!!!!!!!!!
        // Pensé qu'il ne faut mieux ne pas réaffecter la variable Books, mais juste lui ajouter et / ou enlever des éléments
        // Donc pas de LibraryService.Instance.Books = ...
        // mais plutot LibraryService.Instance.Books.Add(...)
        // ou LibraryService.Instance.Books.Clear()
        public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>() {
            new Book(),
            new Book(),
            new Book()
        };
        public async void UpdateBookList()
        {
            var books = await new BookClient(_httpClient).GetBooksAsync("null",0,10);

            Application.Current.Dispatcher.Invoke(() =>
            {
                Books?.Clear();

                foreach (var book in books.OrderBy(b => b.Id))
                {
                    var newbook = new Book()
                    {
                      
                        Price = book.Price,
                        Genres = book.Genres,
                        Id = book.Id,
                        Name = book.Name,
                        Author = book.Author
                    };
                    Books?.Add(newbook);
                }
            });
        }

        // C'est aussi ici que vous ajouterez les requête réseau pour récupérer les livres depuis le web service que vous avez fait
        // Vous pourrez alors ajouter les livres obtenu a la variable Books !
        // Faite bien attention a ce que votre requête réseau ne bloque pas l'interface 
        public ObservableCollection<Genre> Genres { get; set; } = new ObservableCollection<Genre>();

        

        public async void UpdateGenreList()
        {
            var genres = await new BookClient(_httpClient).GetGenresAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Genres?.Clear();
                foreach (var genre in genres.ToList())
                {
                    Genres?.Add(genre);
                }
            });
        }
    }
}
