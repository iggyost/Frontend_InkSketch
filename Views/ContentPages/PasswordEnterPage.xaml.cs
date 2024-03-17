using Frontend_InkSketch.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class PasswordEnterPage : ContentPage
{
    public PasswordEnterPage()
    {
        InitializeComponent();
    }

    private async void passwordEnter_Completed(object sender, EventArgs e)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync($"{App.conString}users/enter/{App.enteredPhone}/{passwordEnter.Text}");
        if (response.IsSuccessStatusCode)
        {
            HttpClient dataClient = new HttpClient();
            HttpResponseMessage dataResponse = await dataClient.GetAsync($"{App.conString}users/get/data/{App.enteredPhone}");

            if (dataResponse.IsSuccessStatusCode)
            {
                string dataContent = await dataResponse.Content.ReadAsStringAsync();
                var dataUser = JsonConvert.DeserializeObject<User>(dataContent);
                App.enteredUser = dataUser;
            }

            App.Current.MainPage = new AppShell();
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            await DisplayAlert("Ошибка!", "Неверный пароль!", "Закрыть");
        }
        else
        {
            await DisplayAlert("Ошибка!", "Ошибка сервера!", "Закрыть");
        }
    }

    private async void enterBtn_Clicked(object sender, EventArgs e)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync($"{App.conString}users/enter/{App.enteredPhone}/{passwordEnter.Text}");
        if (response.IsSuccessStatusCode)
        {
            HttpClient dataClient = new HttpClient();
            HttpResponseMessage dataResponse = await dataClient.GetAsync($"{App.conString}users/get/data/{App.enteredPhone}");

            if (dataResponse.IsSuccessStatusCode)
            {
                string dataContent = await dataResponse.Content.ReadAsStringAsync();
                var dataUser = JsonConvert.DeserializeObject<User>(dataContent);
                App.enteredUser = dataUser;
            }

            App.Current.MainPage = new AppShell();
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            await DisplayAlert("Ошибка!", "Неверный пароль!", "Закрыть");
        }
        else
        {
            await DisplayAlert("Ошибка!", "Ошибка сервера!", "Закрыть");
        }
    }
}