using System.Text.RegularExpressions;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class PhoneEnterPage : ContentPage
{
    public PhoneEnterPage()
    {
        InitializeComponent();
    }

    private async void enterBtn_Clicked(object sender, EventArgs e)
    {
        if (phoneEnter.Text == string.Empty || phoneEnter.Text == null)
        {
            await DisplayAlert("Ошибка!", "Поле 'Номер телефона' не может быть пустым!", "Закрыть");
        }
        else
        {
            var regex = new Regex("^((\\+7|7|8)+([0-9]){10})$");
            if (regex.IsMatch(phoneEnter.Text))
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync($"{App.conString}users/get/{phoneEnter.Text}");

                if (response.IsSuccessStatusCode)
                {
                    App.enteredPhone = phoneEnter.Text;
                    Application.Current.MainPage = new PasswordEnterPage();
                }
                else
                {
                    App.enteredPhone = phoneEnter.Text;
                    Application.Current.MainPage = new RegistrationPage();
                }

            }
            else
            {
                await DisplayAlert("Ошибка!", "Телефон не соответствует формату! ('7 ХХХ ХХХ ХХ ХХ')", "Закрыть");
            }
        }
    }
}