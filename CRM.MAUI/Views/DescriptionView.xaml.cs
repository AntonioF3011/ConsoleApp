namespace CRM.MAUI.Views;
using CRM.MAUI.ViewModels;
[QueryProperty(nameof(ProductId), "productId")] //this is reciving the product Id
public partial class DescriptionView : ContentPage
{
    public int ProductId { get; set; }
    public DescriptionView()
	{
		InitializeComponent();
	}
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ProductViewModel(ProductId);
    }

    private void BackHomeClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }
}