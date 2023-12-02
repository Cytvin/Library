using CommunityToolkit.Maui.Views;
using Library.Popups;
using LibraryDAL;
using LibraryDAL.EFModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Library.ViewModels
{
    internal class MainPageViewModel : BaseViewModel
    {
        private MainPage _view;
        private IDataBaseFacade<Person> _personFacade;
        private IDataBaseFacade<Book> _bookFacade;
        private IDataBaseFacade<BookReservation> _bookReservationFacade;

        private bool _isBookTableEnable = false;
        private bool _isPeopleTableEnable = false;
        private bool _isBookReservationTableEnable = false;

        private ObservableCollection<Person> _people = new ObservableCollection<Person>();
        private ObservableCollection<Book> _books = new ObservableCollection<Book>();
        private ObservableCollection<BookReservation> _bookReservations = new ObservableCollection<BookReservation>();

        public ICommand ShowBookTable { get; private set; }
        public ICommand InsertBook { get; private set; }
        public ICommand UpdateBook { get; private set; }
        public ICommand DeleteBook { get; private set; }
        public ICommand ShowPeopleTable { get; private set; }
        public ICommand InsertPeople { get; private set; }
        public ICommand UpdatePeople { get; private set; }
        public ICommand DeletePeople { get; private set; }
        public ICommand ShowBookReservationTable { get; private set; }
        public ICommand InsertBookReservation { get; private set; }
        public ICommand UpdateBookReservation { get; private set; }
        public ICommand DeleteBookReservation { get; private set; }

        public Book? SelectedBook { get; set; }
        public Person? SelectedPerson { get; set; }
        public BookReservation? SelectedBookReservation { get; set; }
        public bool IsBookTableEnable
        {
            get => _isBookTableEnable;
            set
            {
                _isBookTableEnable = value;
                OnPropertyChanged(this, nameof(IsBookTableEnable));
            }
        }

        public bool IsPeopleTableEnable
        {
            get => _isPeopleTableEnable;
            private set
            {
                _isPeopleTableEnable = value;
                OnPropertyChanged(this, nameof(IsPeopleTableEnable));
            }
        }

        public bool IsBookReservationTableEnable
        {
            get => _isBookReservationTableEnable;
            private set
            {
                _isBookReservationTableEnable = value;
                OnPropertyChanged(this, nameof(IsBookReservationTableEnable));
            }
        }

        public ObservableCollection<Book> Books
        {
            get => _books;
            private set
            {
                _books = value;
                OnPropertyChanged(this, nameof(Books));
            }
        }

        public ObservableCollection<Person> People
        {
            get => _people;
            private set
            {
                _people = value;
                OnPropertyChanged(this, nameof(People));
            }
        }

        public ObservableCollection<BookReservation> BookReservations
        {
            get => _bookReservations;
            private set
            {
                _bookReservations = value;
                OnPropertyChanged(this, nameof(BookReservations));
            }
        }

        public MainPageViewModel(MainPage view, IDataBaseFacade<Person> personFacade,
            IDataBaseFacade<Book> bookFacade, IDataBaseFacade<BookReservation> bookReservationFacade)
        {
            _view = view;
            _personFacade = personFacade;
            _bookFacade = bookFacade;
            _bookReservationFacade = bookReservationFacade;

            ShowBookTable = new Command(OnShowBookTable);
            InsertBook = new Command(OnInsertBook);
            UpdateBook = new Command(OnUpdateBook);
            DeleteBook = new Command(OnDeleteBook);
            ShowPeopleTable = new Command(OnShowPeopleTable);
            InsertPeople = new Command(OnInsertPeople);
            UpdatePeople = new Command(OnUpdatePeople);
            DeletePeople = new Command(OnDeletePeople);
            ShowBookReservationTable = new Command(OnShowBookReservationTable);
            InsertBookReservation = new Command(OnInsertBookReservation);
            UpdateBookReservation = new Command(OnUpdateBookReservation);
            DeleteBookReservation = new Command(OnDeleteBookReservation);
        }

        private void OnShowBookTable()
        {
            IsBookTableEnable = true;
            IsPeopleTableEnable = false;
            IsBookReservationTableEnable = false;
            Books = new ObservableCollection<Book>(_bookFacade.SelectAll());
        }

        private async void OnInsertBook()
        {
            object? book = await _view.ShowPopupAsync(new BookPopup(_bookFacade));

            if (book != null)
            {
                OnShowBookTable();
            }
        }

        private async void OnUpdateBook()
        {
            if (SelectedBook == null)
            {
                await _view.DisplayAlert("Ошибка", "Не выбрана книга", "Отмена");
                return;
            }

            object? book = await _view.ShowPopupAsync(new BookPopup(_bookFacade, SelectedBook));

            if (book != null)
            {
                OnShowBookTable();
            }
        }

        private void OnDeleteBook()
        {
            if (SelectedBook == null)
            {
                _view.DisplayAlert("Ошибка", "Не выбрана книга", "Отмена");
                return;
            }

            _bookFacade.Delete(SelectedBook);
            OnShowBookTable();
        }

        private void OnShowPeopleTable()
        {
            IsBookTableEnable = false;
            IsPeopleTableEnable = true;
            IsBookReservationTableEnable = false;
            People = new ObservableCollection<Person>(_personFacade.SelectAll());
        }

        private async void OnInsertPeople()
        {
            object? person = await _view.ShowPopupAsync(new PersonPopup(_personFacade));

            if (person != null)
            {
                OnShowPeopleTable();
            }
        }

        private async void OnUpdatePeople()
        {
            if (SelectedPerson == null)
            {
                await _view.DisplayAlert("Ошибка", "Не выбран читатель", "Отмена");
                return;
            }

            object? person = await _view.ShowPopupAsync(new PersonPopup(_personFacade, SelectedPerson));

            if (person != null)
            {
                OnShowPeopleTable();
            }
        }

        private void OnDeletePeople()
        {
            if (SelectedPerson == null)
            {
                _view.DisplayAlert("Ошибка", "Не выбран читатель", "Отмена");
                return;
            }

            _personFacade.Delete(SelectedPerson);

            OnShowPeopleTable();
        }

        private void OnShowBookReservationTable()
        {
            IsBookTableEnable = false;
            IsPeopleTableEnable = false;
            IsBookReservationTableEnable = true;
            BookReservations = new ObservableCollection<BookReservation>(_bookReservationFacade.SelectAll());
        }

        private void OnInsertBookReservation()
        {

        }

        private void OnUpdateBookReservation()
        {
            if (SelectedBookReservation == null)
            {
                _view.DisplayAlert("Ошибка", "Запись не выбрана", "Отмена");
                return;
            }


        }

        private void OnDeleteBookReservation()
        {
            if (SelectedBookReservation == null)
            {
                _view.DisplayAlert("Ошибка", "Запись не выбрана", "Отмена");
                return;
            }


        }
    }
}
