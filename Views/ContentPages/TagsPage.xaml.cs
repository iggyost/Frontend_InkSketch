using Frontend_InkSketch.ApplicationData;
using Newtonsoft.Json;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class TagsPage : ContentPage
{
	public TagsPage()
	{
		InitializeComponent();
        GetTags();
	}
    public static List<FavoritesView> favorites = new List<FavoritesView>();
    public static Tag selectedTag = new Tag();
    public async void GetTattoes(int tagId)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}imagesview/get/tag/{tagId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            imagesCv.ItemsSource = JsonConvert.DeserializeObject<ImagesView[]>(content).ToList();
        }
    }
    public async void GetTags()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}tags/get");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            categoryPicker.ItemsSource = JsonConvert.DeserializeObject<Tag[]>(content).ToList();
        }
    }
    private void categoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        selectedTag = categoryPicker.SelectedItem as Tag;
        GetTattoes(selectedTag.TagId);
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {

    }

    private async void favoriteBtn_Clicked(object sender, EventArgs e)
    {
        ImageButton imageButton = (ImageButton)sender;
        var imageButtonId = int.Parse(imageButton.AutomationId);
        var IsImageFavorite = favorites.Where(x => x.ImageId == imageButtonId && x.UserId == App.enteredUser.UserId).FirstOrDefault();
        if (IsImageFavorite == null)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{App.conString}favorites/add/{App.enteredUser.UserId}/{imageButtonId}");
            if (response.IsSuccessStatusCode)
            {
                HttpClient refClient = new HttpClient();
                var refResponse = await refClient.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
                if (refResponse.IsSuccessStatusCode)
                {
                    string refContent = await refResponse.Content.ReadAsStringAsync();
                    favorites = JsonConvert.DeserializeObject<FavoritesView[]>(refContent).ToList();
                }
                imageButton.Source = "favorite_icon_filled.png";
            }
        }
        else
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{App.conString}favorites/remove/{App.enteredUser.UserId}/{imageButtonId}");
            if (response.IsSuccessStatusCode)
            {
                HttpClient refClient = new HttpClient();
                var refResponse = await refClient.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
                if (refResponse.IsSuccessStatusCode)
                {
                    string refContent = await refResponse.Content.ReadAsStringAsync();
                    favorites = JsonConvert.DeserializeObject<FavoritesView[]>(refContent).ToList();
                }
                imageButton.Source = "favorite_icon_empty.png";
            }
        }
    }

    private async void favoriteBtn_Loaded(object sender, EventArgs e)
    {
        ImageButton imageButton = (ImageButton)sender;
        var imageButtonId = int.Parse(imageButton.AutomationId);
        var IsImageFavorite = favorites.Where(x => x.ImageId == imageButtonId && x.UserId == App.enteredUser.UserId).FirstOrDefault();
        if (IsImageFavorite != null)
        {
            imageButton.Source = "favorite_icon_filled.png";
        }
        else
        {
            imageButton.Source = "favorite_icon.png";
        }
    }

    private void refreshCv_Refreshing(object sender, EventArgs e)
    {
        if (selectedTag != null)
        {
            refreshCv.IsRefreshing = true;
            GetTattoes(selectedTag.TagId);
            refreshCv.IsRefreshing = false;
        }
    }
}