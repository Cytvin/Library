using CommunityToolkit.Maui.Core.Extensions;
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
        private bool _isSearchInBookReservation = false;

        private ObservableCollection<Person> _people = new ObservableCollection<Person>();
        private ObservableCollection<Book> _books = new ObservableCollection<Book>();
        private ObservableCollection<BookReservation> _bookReservations = new ObservableCollection<BookReservation>();

        private ICommand? _search;

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
        public ICommand Search
        {
            get => _search;
            set
            {
                _search = value;
                OnPropertyChanged(this, nameof(Search));
            }
        }

        public Book? SelectedBook { get; set; }
        public Person? SelectedPerson { get; set; }
        public BookReservation? SelectedBookReservation { get; set; }
        public int BookResevationSearchTypeIndex { get; set; }
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

        public bool IsSearchInBookReservation
        {
            get => _isSearchInBookReservation;
            set
            {
                _isSearchInBookReservation = value;
                OnPropertyChanged(this, nameof(IsSearchInBookReservation));
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
            IsSearchInBookReservation = false;
            Books = new ObservableCollection<Book>(_bookFacade.SelectAll());
            Search = new Command<string>(OnSearchBook);
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

        private void OnSearchBook(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                Books = _bookFacade.SelectAll().ToObservableCollection();
                return;
            }

            Books = _bookFacade.SelectAll().Where(b => b.Name.ToLower().Contains(searchString.ToLower())).ToObservableCollection();
        }

        private void OnShowPeopleTable()
        {
            IsBookTableEnable = false;
            IsPeopleTableEnable = true;
            IsBookReservationTableEnable = false;
            IsSearchInBookReservation = false;
            People = new ObservableCollection<Person>(_personFacade.SelectAll());
            Search = new Command<string>(OnSearchPerson);
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

        private void OnSearchPerson(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                People = _personFacade.SelectAll().ToObservableCollection();
                return;
            }

            People = _personFacade.SelectAll().Where(p => p.Fio.ToLower().Contains(searchString.ToLower())).ToObservableCollection();
        }

        private void OnShowBookReservationTable()
        {
            IsBookTableEnable = false;
            IsPeopleTableEnable = false;
            IsBookReservationTableEnable = true;
            IsSearchInBookReservation = true;
            BookReservations = new ObservableCollection<BookReservation>(_bookReservationFacade.SelectAll());
            Search = new Command<string>(OnSearchBookReservation);
        }

        private async void OnInsertBookReservation()
        {
            IEnumerable<Person> personList = _personFacade.SelectAll();
            IEnumerable<Book> bookList = _bookFacade.SelectAll().Where(br => br.BookReservations.Count == 0 || br.BookReservations == null).ToList();

            if (bookList.Count() == 0)
            {
                await _view.DisplayAlert("Ошибка", "Нет книг, доступных к выдаче", "Отмена");
                return;
            }

            object? bookReservation = await _view.ShowPopupAsync(new BookReservationPopup(personList, bookList, _bookReservationFacade));

            if (bookReservation != null)
            {
                OnShowBookReservationTable();
            }
        }

        private async void OnUpdateBookReservation()
        {
            if (SelectedBookReservation == null)
            {
                await _view.DisplayAlert("Ошибка", "Запись не выбрана", "Отмена");
                return;
            }

            IEnumerable<Person> personList = _personFacade.SelectAll();
            IEnumerable<Book> bookList = _bookFacade.SelectAll();

            object? bookReservation = await _view.ShowPopupAsync(new BookReservationPopup(personList, bookList, _bookReservationFacade, SelectedBookReservation));

            if (bookReservation != null)
            {
                OnShowBookReservationTable();
            }
        }

        private void OnDeleteBookReservation()
        {
            if (SelectedBookReservation == null)
            {
                _view.DisplayAlert("Ошибка", "Запись не выбрана", "Отмена");
                return;
            }

            _bookReservationFacade.Delete(SelectedBookReservation);
            OnShowBookReservationTable();
        }

        private void OnSearchBookReservation(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                BookReservations = _bookReservationFacade.SelectAll().ToObservableCollection();
                return;
            }

            if (BookResevationSearchTypeIndex == 0)
            {
                BookReservations = _bookReservationFacade.SelectAll().Where(br => br.People.Fio.ToLower().Contains(searchString.ToLower())).ToObservableCollection();
            }
            else
            {
                BookReservations = _bookReservationFacade.SelectAll().Where(br => br.Book.Name.ToLower().Contains(searchString.ToLower())).ToObservableCollection();
            } 
        }
    }
}
