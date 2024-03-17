namespace Frontend_InkSketch.Views.ContentPages;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		InitializeComponent();

	}

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
		await Task.Delay(5000);
		App.Current.MainPage = new PhoneEnterPage();
    }
}