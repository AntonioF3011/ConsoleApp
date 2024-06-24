using CRM.MAUI.ViewModels;

namespace CRM.MAUI.Views;

public partial class CartView : ContentPage
{
	public CartView()
	{
		InitializeComponent();
		BindingContext = new CartViewModel();
	}

    private void BackClicked(object sender, EventArgs e)
    {

		Shell.Current.GoToAsync("//MainPage");
    }
}