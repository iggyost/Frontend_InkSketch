using Frontend_InkSketch.ApplicationData;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        DataList.Clear();
        categoriesList.Clear();
        favorites.Clear();
        selectedCategory = 0;
        skipCount = 0;
        imagesCv.ItemsSource = null;
    }
    public ObservableCollection<ImagesView> DataList { get; set; } = new ObservableCollection<ImagesView>();
    public static List<Category> categoriesList = new List<Category>();
    public static List<FavoritesView> favorites = new List<FavoritesView>();
    public static int selectedCategory = 0;
    public static int skipCount = 0;
    public async void LoadCategories()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}categories/get");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            categoriesList = JsonConvert.DeserializeObject<Category[]>(content).ToList();
            Category categoryAll = new Category()
            {
                CategoryId = 0,
                Name = "Все"
            };
            categoriesList.Add(categoryAll);
            categoryPicker.ItemsSource = categoriesList;
        }
    }
    public async void LoadTattoes()
    {
        AnimationLoading(true);

        try
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{App.conString}imagesview/get/rec/{App.enteredUser.UserId}/{skipCount}/{selectedCategory}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var imagesList = JsonConvert.DeserializeObject<ImagesView[]>(content).ToList();
                foreach (var item in imagesList)
                {
                    DataList.Add(item);
                }
                imagesCv.ItemsSource = DataList;
            }
        }
        catch (Exception)
        {

        }

        AnimationLoading(false);
    }
    public async void AnimationLoading(bool isLoading)
    {
        if (isLoading)
        {
            cvFooterActInd.IsRunning = true;
            imagesCv.IsEnabled = false;
            IsBusy = true;
        }
        else
        {
            cvFooterActInd.IsRunning = false;
            imagesCv.IsEnabled = true;
            IsBusy = false;
        }
    }
    private void imagesCv_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        skipCount = skipCount + 4;
        LoadTattoes();
    }
    private void categoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedItem = categoryPicker.SelectedItem as Category;
        if (selectedItem != null)
        {
            selectedCategory = selectedItem.CategoryId;
            DataList.Clear();
            imagesCv.ItemsSource = null;
            skipCount = 0;
            LoadTattoes();
        }
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

    private void favoriteBtn_Loaded(object sender, EventArgs e)
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

    private void TagBorder_Loaded(object sender, EventArgs e)
    {
        Border border = (Border)sender;
        var borderId = int.Parse(border.AutomationId);
        var selectedImage = DataList.Where(x => x.ImageId == borderId).FirstOrDefault();
        if (selectedImage != null)
        {
            border.Background = Color.FromArgb(selectedImage.HexColor);
        }
    }

    private void NameTagLabel_Loaded(object sender, EventArgs e)
    {
        Label label = (Label)sender;
        var labelId = int.Parse(label.AutomationId);
        var selectedImage = DataList.Where(x => x.ImageId == labelId).FirstOrDefault();
        if (selectedImage != null)
        {
            label.Text = "#" + selectedImage.Name;
        }
    }

    private async void homePage_Loaded(object sender, EventArgs e)
    {
        LoadCategories();
        LoadTattoes();
        HttpClient client = new HttpClient();
        var response = await client.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            favorites = JsonConvert.DeserializeObject<FavoritesView[]>(content).ToList();
        }
    }

    private async void refreshCv_Refreshing(object sender, EventArgs e)
    {
        refreshCv.IsRefreshing = true;
        AnimationLoading(true);

        DataList.Clear();
        imagesCv.ItemsSource = null;
        skipCount = 0;
        favorites.Clear();

        if (skipCount == 0)
        {
            HttpClient refClient = new HttpClient();
            var refResponse = await refClient.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
            if (refResponse.IsSuccessStatusCode)
            {
                string refContent = await refResponse.Content.ReadAsStringAsync();
                favorites = JsonConvert.DeserializeObject<FavoritesView[]>(refContent).ToList();
            }

            LoadTattoes();
        }
        else
        {

        }

        AnimationLoading(false);
        refreshCv.IsRefreshing = false;
    }
}