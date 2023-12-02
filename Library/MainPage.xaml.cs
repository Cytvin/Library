using LibraryDAL.EFModels;
using LibraryDAL;
using Library.ViewModels;

namespace Library
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IDataBaseFacade<Person> personFacade, IDataBaseFacade<Book> bookFacade, IDataBaseFacade<BookReservation> bookReservationFacade)
        {
            InitializeComponent();
            
            BindingContext = new MainPageViewModel(this, personFacade, bookFacade, bookReservationFacade);
        }
    }
}
