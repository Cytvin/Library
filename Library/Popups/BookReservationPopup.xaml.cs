using CommunityToolkit.Maui.Views;
using LibraryDAL;
using LibraryDAL.EFModels;
using Library.ViewModels;

namespace Library.Popups;

public partial class BookReservationPopup : Popup
{
	public BookReservationPopup(IEnumerable<Person> personList, IEnumerable<Book> bookList,
        IDataBaseFacade<BookReservation> bookReservationFasade)
	{
		InitializeComponent();
        InitializeParameters();

        BindingContext = new BookReservationViewModel(this, personList, bookList, bookReservationFasade);
	}

    public BookReservationPopup(IEnumerable<Person> personList, IEnumerable<Book> bookList,
        IDataBaseFacade<BookReservation> bookReservationFasade, BookReservation reservation)
    {
        InitializeComponent();
        InitializeParameters();

        BindingContext = new BookReservationViewModel(this, personList, bookList, bookReservationFasade, reservation);
    }

    private void InitializeParameters()
    {
        DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;
        PopupWindow.WidthRequest = displayInfo.Width / 4;
        PopupWindow.HeightRequest = displayInfo.Height / 4;

        EntryList.Height = displayInfo.Height / 4 - 125;
    }

    private void ButtonClose_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}