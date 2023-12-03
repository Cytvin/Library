using LibraryDAL.EFModels;
using LibraryDAL;
using System.Windows.Input;
using Library.Popups;

namespace Library.ViewModels
{
    internal class BookReservationViewModel : BaseViewModel
    {
        private string _title;
        private string _saveButtonTitle;
        private IDataBaseFacade<BookReservation> _bookReservationFacade;
        private BookReservation? _bookReservation;
        private BookReservationPopup _view;

        public string Title => _title;
        public string SaveButtonTitle => _saveButtonTitle;
        public Person? SelectedPeople { get; set; }
        public Book? SelectedBook { get; set; }
        public IEnumerable<Person> People { get; private set; }
        public IEnumerable<Book> Books { get; private set; }

        public ICommand Save { get; private set; }

        public BookReservationViewModel(BookReservationPopup view, IEnumerable<Person> personList,
            IEnumerable<Book> bookList, IDataBaseFacade<BookReservation> bookReservationFacade)
        {
            _view = view;
            _title = "Добавление записи";
            _saveButtonTitle = "Добавить";
            _bookReservationFacade = bookReservationFacade;

            People = personList;
            Books = bookList;

            Save = new Command(OnInsertSave);
        }

        public BookReservationViewModel(BookReservationPopup view, IEnumerable<Person> personList,
            IEnumerable<Book> bookList, IDataBaseFacade<BookReservation> bookReseravtionFacade, BookReservation bookReseravation)
        {
            _view = view;
            _title = "Редактирование записи";
            _saveButtonTitle = "Сохранить";
            _bookReservationFacade = bookReseravtionFacade;
            _bookReservation = bookReseravation;

            People = personList;
            Books = bookList;

            SelectedBook = bookReseravation.Book;
            SelectedPeople = bookReseravation.People;

            Save = new Command(OnUpdateSave);
        }

        private void OnInsertSave()
        {
            if(SelectedBook == null || SelectedPeople == null)
            {
                App.Current?.MainPage?.DisplayAlert("Ошибка", "Не выбраны книга или читатель.", "Отмена");
                return;
            }

            BookReservation bookReservation = new BookReservation();
            bookReservation.People = SelectedPeople;
            bookReservation.Book = SelectedBook;
            bookReservation.Date = DateOnly.FromDateTime(DateTime.Now);

            _bookReservationFacade.Insert(bookReservation);
            _view.Close(bookReservation);
        }


        private void OnUpdateSave()
        {
            if (SelectedBook == null || SelectedPeople == null)
            {
                App.Current?.MainPage?.DisplayAlert("Ошибка", "Не выбраны книга или читатель.", "Отмена");
                return;
            }

            if (_bookReservation == null)
            {
                throw new ArgumentNullException(nameof(_bookReservation), "Объект пользователя должен быть определен для редактирования");
            }

            _bookReservation.People = SelectedPeople;
            _bookReservation.Book = SelectedBook;
            _bookReservationFacade.Update(_bookReservation);
            _view.Close(_bookReservation);
        }
    }
}
