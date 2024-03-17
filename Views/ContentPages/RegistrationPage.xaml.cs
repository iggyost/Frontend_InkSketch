using Frontend_InkSketch.ApplicationData;
using System.Net.Http.Json;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage()
    {
        InitializeComponent();
        phoneEntry.Text = App.enteredPhone;
    }

    private void nameEntry_Completed(object sender, EventArgs e)
    {

    }

    private void passwordEntry_Completed(object sender, EventArgs e)
    {

    }

    private async void enterBtn_Clicked(object sender, EventArgs e)
    {
        if (passwordEntry.Text == string.Empty || passwordEntry.Text == null || passwordEntry.Placeholder != null && passwordEntry.Text.Length <= 3)
        {
            await DisplayAlert("Ошибка!", "Введите пароль (не менее 4 символов)", "Закрыть");
        }
        else
        {
            User newUser = new User()
            {
                Name = nameEntry.Text,
                Password = passwordEntry.Text,
                Phone = App.enteredPhone,
                RegistrationDate = DateTime.Now
            };
            HttpClient client = new HttpClient();
            var response = await client.PostAsJsonAsync($"{App.conString}users/reg", newUser);
            if (response.IsSuccessStatusCode)
            {
                App.enteredUser = newUser;
                Application.Current.MainPage = new QuestionsPage();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                await DisplayAlert("Ошибка!", "Возникло исключение при регистрации нового пользователя! ", "Закрыть");
            }
            else
            {
                await DisplayAlert("Ошибка", "Ошибка сервера!", "Закрыть");
            }
        }
    }
}