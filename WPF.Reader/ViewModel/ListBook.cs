using CommunityToolkit.Mvvm.DependencyInjection;
using MyNamespace;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Windows.Input;
using WPF.Reader.Service;





namespace WPF.Reader.ViewModel
{
    internal class ListBook : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HttpClient _httpClient = new () { BaseAddress = new Uri("http://127.0.0.1:5000") };
        public ICommand ItemSelectedCommand { get; set; }




        // n'oublier pas faire de faire le binding dans ListBook.xaml !!!!
        public ObservableCollection<Genre> Genres => Ioc.Default.GetRequiredService<LibraryService>().Genres;

        public ObservableCollection<Book> Books => Ioc.Default.GetRequiredService<LibraryService>().Books;

        public ListBook()
        {
            //ItemSelectedCommand = new RelayCommand(book => { /* the livre devrais etre dans la variable book */ });
            Ioc.Default.GetRequiredService<LibraryService>().UpdateBookList();
            Ioc.Default.GetRequiredService<LibraryService>().UpdateGenreList();

            ItemSelectedCommand = new RelayCommand(book => {
                Ioc.Default.GetRequiredService<INavigationService>().Navigate<DetailsBook>(book);
            });

        }

    }
}

