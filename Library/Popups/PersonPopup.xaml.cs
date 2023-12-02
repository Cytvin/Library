using CommunityToolkit.Maui.Views;
using Library.ViewModels;
using LibraryDAL;
using LibraryDAL.EFModels;

namespace Library.Popups;

public partial class PersonPopup : Popup
{
	public PersonPopup(IDataBaseFacade<Person> personFacade)
	{
		InitializeComponent();
        InitializeParameters();

        BindingContext = new PersonViewModel(this, personFacade);
	}

    public PersonPopup(IDataBaseFacade<Person> personFacade, Person person)
    {
        InitializeComponent();
        InitializeParameters();

        BindingContext = new PersonViewModel(this, personFacade, person);
    }

    private void InitializeParameters()
    {
        DisplayInfo displayInfo = DeviceDisplay.MainDisplayInfo;
        PopupWindow.WidthRequest = displayInfo.Width / 4;
        PopupWindow.HeightRequest = displayInfo.Height / 5;

        EntryList.Height = displayInfo.Height / 5 - 125;
    }

    private void ButtonClose_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}