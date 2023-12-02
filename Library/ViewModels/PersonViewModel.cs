using LibraryDAL;
using LibraryDAL.EFModels;
using System.Windows.Input;
using Library.Popups;

namespace Library.ViewModels
{
    internal class PersonViewModel : BaseViewModel
    {
        private string _title;
        private string _saveButtonTitle;
        private IDataBaseFacade<Person> _personFacade;
        private Person? _person;
        private PersonPopup _view;

        public string Title => _title;
        public string SaveButtonTitle => _saveButtonTitle;
        public string? Fio { get; set; }

        public ICommand Save { get; private set; }

        public PersonViewModel(PersonPopup view, IDataBaseFacade<Person> personFacade)
        {
            _view = view;
            _title = "Добавление читателя";
            _saveButtonTitle = "Добавить";
            _personFacade = personFacade;

            Save = new Command(OnInsertSave);
        }

        public PersonViewModel(PersonPopup view, IDataBaseFacade<Person> personFacade, Person person)
        {
            _view = view;
            _title = "Редактирование читателя";
            _saveButtonTitle = "Сохранить";
            _personFacade = personFacade;
            _person = person;

            Fio = person.Fio;
            Save = new Command(OnUpdateSave);
        }

        private void OnInsertSave()
        {
            if (IsFioEntryCorrect() == false)
            {
                App.Current?.MainPage?.DisplayAlert("Ошибка", "Не заполнено поле ФИО", "Отмена");
                return;
            }

            Person person = new Person();
            person.Fio = Fio;

            _personFacade.Insert(person);
            _view.Close(person);
        }


        private void OnUpdateSave()
        {
            if (IsFioEntryCorrect() == false) 
            {
                App.Current?.MainPage?.DisplayAlert("Ошибка", "Не заполнено поле ФИО", "Отмена");
                return;
            }

            if (_person == null)
            {
                throw new ArgumentNullException(nameof(_person), "Объект пользователя должен быть определен для редактирования");
            }

            _person.Fio = Fio;
            _personFacade.Update(_person);
            _view.Close(_person);
        }

        private bool IsFioEntryCorrect()
        {
            if (Fio == null)
            {
                return false;
            }

            if (Fio.Length == 0)
            {
                return false;
            }

            return true;
        }
    }
}
