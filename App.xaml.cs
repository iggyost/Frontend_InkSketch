using Frontend_InkSketch.ApplicationData;
using Frontend_InkSketch.Views.ContentPages;

namespace Frontend_InkSketch;

public partial class App : Application
{
    public static string conString = "http://192.168.0.10:45455/api/";
	public static User enteredUser;
	public static string enteredPhone;
    public App()
	{
		InitializeComponent();

		MainPage = new WelcomePage();
	}
}
