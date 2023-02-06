using System;
using System.ComponentModel;
using WPF.Reader.Model;

namespace WPF.Reader.ViewModel
{
    class ReadBook : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // A vous de jouer maintenant
        public Object CurrentBook { get; init; }
        public ReadBook(Object book)
        {
            CurrentBook= book;
        }
           
    }

    /* Cette classe sert juste a afficher des donnée de test dans le designer */
    class InDesignReadBook : ReadBook
    {
        public InDesignReadBook() : base( new Book() { Name = "Book Test"})
        {
        }
    }
}
