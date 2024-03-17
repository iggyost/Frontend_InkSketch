using Frontend_InkSketch.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class FavoritesPage : ContentPage
{
    public FavoritesPage()
    {
        InitializeComponent();
    }

    private async void imagesCv_Loaded(object sender, EventArgs e)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var favorites = JsonConvert.DeserializeObject<FavoritesView[]>(content).ToList();
            imagesCv.ItemsSource = favorites;
        }
    }

    private async void SwipeItem_Invoked(object sender, EventArgs e)
    {
        SwipeItem swipeItem = (SwipeItem)sender;
        var swipeItemId = int.Parse(swipeItem.AutomationId);
        HttpClient client = new HttpClient();
        var response = await client.GetAsync($"{App.conString}favorites/remove/{App.enteredUser.UserId}/{swipeItemId}");
        if (response.IsSuccessStatusCode)
        {
            HttpClient refClient = new HttpClient();
            var refResponse = await refClient.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
            if (refResponse.IsSuccessStatusCode)
            {
                string refContent = await refResponse.Content.ReadAsStringAsync();
                var refFavorites = JsonConvert.DeserializeObject<FavoritesView[]>(refContent).ToList();
                imagesCv.ItemsSource = refFavorites;
            }
        }
    }

    private async void refreshCv_Refreshing(object sender, EventArgs e)
    {
        refreshCv.IsRefreshing = true;
        HttpClient client = new HttpClient();
        var response = await client.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var favorites = JsonConvert.DeserializeObject<FavoritesView[]>(content).ToList();
            imagesCv.ItemsSource = favorites;
        }
        refreshCv.IsRefreshing = false;
    }
}