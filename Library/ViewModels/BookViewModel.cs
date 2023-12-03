using LibraryDAL.EFModels;
using LibraryDAL;
using System.Windows.Input;
using Library.Popups;

namespace Library.ViewModels
{
    internal class BookViewModel : BaseViewModel
    {
        private string _title;
        private string _saveButtonTitle;
        private IDataBaseFacade<Book> _bookFacade;
        private Book? _book;
        private BookPopup _view;

        public string Title => _title;
        public string SaveButtonTitle => _saveButtonTitle;
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? PublishingYear { get; set; }

        public ICommand Save { get; private set; }

        public BookViewModel(BookPopup view, IDataBaseFacade<Book> bookFacade)
        {
            _view = view;
            _title = "Добавление книгу";
            _saveButtonTitle = "Добавить";
            _bookFacade = bookFacade;

            Save = new Command(OnInsertSave);
        }

        public BookViewModel(BookPopup view, IDataBaseFacade<Book> bookFacade, Book book)
        {
            _view = view;
            _title = "Редактирование книги";
            _saveButtonTitle = "Сохранить";
            _bookFacade = bookFacade;
            _book = book;

            Name = book.Name;
            Author = book.Author;
            PublishingYear = book.PublishingYear.ToString();
            Save = new Command(OnUpdateSave);
        }

        private void OnInsertSave()
        {
            if (IsAllEntriesFillCorrect() == false)
            {
                App.Current?.MainPage?.DisplayAlert("Ошибка", "Не все поля заполнены корректно", "Отмена");
                return;
            }

            Book book = new Book();
            book.Name = Name;
            book.Author = Author;
            book.PublishingYear = Convert.ToInt32(PublishingYear);

            _bookFacade.Insert(book);
            _view.Close(book);
        }


        private void OnUpdateSave()
        {
            if (_book == null)
            {
                throw new ArgumentNullException(nameof(_book), "Объект пользователя должен быть определен для редактирования");
            }

            if (IsAllEntriesFillCorrect() == false)
            {
                App.Current?.MainPage?.DisplayAlert("Ошибка", "Не все поля заполнены корректно", "Отмена");
                return;
            }

            _book.Name = Name;
            _book.Author = Author;
            _book.PublishingYear = Convert.ToInt32(PublishingYear);
            _bookFacade.Update(_book);
            _view.Close(_book);
        }

        private bool IsAllEntriesFillCorrect()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Author) && int.TryParse(PublishingYear, out _);
        }
    }
}
