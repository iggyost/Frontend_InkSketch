using Frontend_InkSketch.ApplicationData;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class ProfilePage : ContentPage
{
    public static List<ProfileView> currentUserTags = new List<ProfileView>();
    public ProfilePage()
    {
        InitializeComponent();
        userNameLbl.Text = App.enteredUser.Name;
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Изменение имени", "Введите имя пользователя");
        if (result != null)
        {
            HttpClient client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(result));
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var response = await client.PutAsync($"{App.conString}users/name/{App.enteredUser.UserId}/{result}", content);
            if (response.IsSuccessStatusCode)
            {
                userNameLbl.Text = result;
            }
        }
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync($"{App.conString}profilesview/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var userTags = JsonConvert.DeserializeObject<ProfileView[]>(content);
            tagsCv.ItemsSource = userTags.ToList();
        }
    }

    private async void exitBtn_Clicked(object sender, EventArgs e)
    {
        var result = await DisplayActionSheet("Выйти из аккаунта?", "Закрыть", "Выйти");
        if (result == "Выйти")
        {
            Application.Current.MainPage = new WelcomePage();
            App.enteredUser = null;
            App.enteredPhone = null;
        }
    }
}