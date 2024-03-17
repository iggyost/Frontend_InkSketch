using Frontend_InkSketch.ApplicationData;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();

    }
    public static List<ImagesView> imagesViews = new List<ImagesView>();
    public ObservableCollection<ImagesView> DataList { get; set; } = new ObservableCollection<ImagesView>();
    public static int thresHoldCount;
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }
    private async void imagesCv_Loaded(object sender, EventArgs e)
    {
        SetContentLoading();

        if (thresHoldCount == 0)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync($"{App.conString}imagesview/get/rec/{App.enteredUser.UserId}/{0}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var newImageView = JsonConvert.DeserializeObject<ImagesView[]>(content);
                imagesViews.AddRange(newImageView);
                foreach (ImagesView imageView in imagesViews)
                {
                    DataList.Add(imageView);
                }
                imagesCv.ItemsSource = DataList;
                //imagesCv.ItemsSource = imagesViews.ToList();
            }
        }
        else
        {

        }

        ContentLoaded();
    }

    private async void imagesCv_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        SetContentLoading();
        try
        {
            thresHoldCount = thresHoldCount + 4;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{App.conString}imagesview/get/rec/{App.enteredUser.UserId}/{thresHoldCount}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var newImageView = JsonConvert.DeserializeObject<ImagesView[]>(content);
                foreach (var item in newImageView)
                {
                    DataList.Add(item);
                }
                imagesCv.ItemsSource = DataList;
                //imagesCv.ItemsSource = imagesViews.ToList();
            }
        }
        catch (Exception)
        {

        }
        ContentLoaded();
    }
    public void SetContentLoading()
    {
        cvFooterActInd.IsRunning = true;
        imagesCv.IsEnabled = false;
        IsBusy = true;
    }
    public void ContentLoaded()
    {
        cvFooterActInd.IsRunning = false;
        imagesCv.IsEnabled = true;
        IsBusy = false;
    }

    private void Border_Loaded(object sender, EventArgs e)
    {
        Border border = (Border)sender;
        var borderId = int.Parse(border.AutomationId);
        var selectedImage = DataList.Where(x => x.ImageId == borderId).FirstOrDefault();
        if (selectedImage != null)
        {
            border.Background = Color.FromArgb(selectedImage.HexColor);
        }
    }

    private void Label_Loaded(object sender, EventArgs e)
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
        HttpClient client = new HttpClient();
        var response = await client.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            favorites = JsonConvert.DeserializeObject<FavoritesView[]>(content).ToList();
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

    public static List<FavoritesView> favorites = new List<FavoritesView>();
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

    private async void refreshCv_Refreshing(object sender, EventArgs e)
    {
        refreshCv.IsRefreshing = true;
        SetContentLoading();

        DataList.Clear();
        imagesViews.Clear();
        thresHoldCount = 0;
        favorites.Clear();

        if (thresHoldCount == 0)
        {
            HttpClient refClient = new HttpClient();
            var refResponse = await refClient.GetAsync($"{App.conString}favoritesview/get/{App.enteredUser.UserId}");
            if (refResponse.IsSuccessStatusCode)
            {
                string refContent = await refResponse.Content.ReadAsStringAsync();
                favorites = JsonConvert.DeserializeObject<FavoritesView[]>(refContent).ToList();
            }

            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{App.conString}imagesview/get/rec/{App.enteredUser.UserId}/{0}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var newImageView = JsonConvert.DeserializeObject<ImagesView[]>(content);
                imagesViews.AddRange(newImageView);
                foreach (ImagesView imageView in imagesViews)
                {
                    DataList.Add(imageView);
                }


                imagesCv.ItemsSource = DataList;
                //imagesCv.ItemsSource = imagesViews.ToList();
            }
        }
        else
        {

        }

        ContentLoaded();
        refreshCv.IsRefreshing = false;
    }
}