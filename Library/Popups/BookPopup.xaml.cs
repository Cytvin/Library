using CommunityToolkit.Maui.Views;
using LibraryDAL;
using LibraryDAL.EFModels;
using Library.ViewModels;

namespace Library.Popups;

public partial class BookPopup : Popup
{
	public BookPopup(IDataBaseFacade<Book> bookFacade)
	{
		InitializeComponent();
        InitializeParameters();

        BindingContext = new BookViewModel(this, bookFacade);
    }

    public BookPopup(IDataBaseFacade<Book> bookFacade, Book book)
    {
        InitializeComponent();
        InitializeParameters();

        BindingContext = new BookViewModel(this, bookFacade, book);
    }

    private void InitializeParameters()
    {
        DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;
        PopupWindow.WidthRequest = displayInfo.Width / 4;
        PopupWindow.HeightRequest = displayInfo.Height / 3;

        EntryList.Height = displayInfo.Height / 3 - 125;
    }

    private void ButtonClose_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}