using MyNamespace;
using System.ComponentModel;
using System.Windows.Input;

using System;
using System.Net.Http;
using CommunityToolkit.Mvvm.DependencyInjection;
using WPF.Reader.Service;

namespace WPF.Reader.ViewModel
{
    public class DetailsBook : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://127.0.0.1:5000") };

        // n'oublier pas faire de faire le binding dans DetailsBook.xaml !!!!

        public Book CurrentBook { get; set; }

        public ICommand ReadCommand { get; init; } = new RelayCommand(book => { Ioc.Default.GetRequiredService<INavigationService>().Navigate<ReadBook>((Book)book); });

        public DetailsBook(Book  book)
        {
            GetDetailsBook (book.Id);
        }
        
        public async void GetDetailsBook (int id)
        {
            var currentBook = await new BookClient (_httpClient).GetBookAsync(id);
            CurrentBook = currentBook; 

        }
    }

    /* Cette classe sert juste a afficher des donnée de test dans le designer */
    public class InDesignDetailsBook : DetailsBook
    {
        public InDesignDetailsBook() : base(new Book() /*{ Title = "Test Book" }*/) { }
    }
}

