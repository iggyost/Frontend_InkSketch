using Frontend_InkSketch.ApplicationData;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;
using Image = Microsoft.Maui.Controls.Image;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class QuestionsPage : ContentPage
{

    public QuestionsPage()
    {
        InitializeComponent();
        firstTag = 0;
        secondTag = 0;
        firstDataList.Clear();
        secondDataList.Clear();
        imagesList.Clear();
        isFirstSelected = false;
        isSecondSelected = false;
    }
    public static List<ImagesView> imagesList = new List<ImagesView>();
    public ObservableCollection<ImagesView> firstDataList { get; set; } = new ObservableCollection<ImagesView>();
    public ObservableCollection<ImagesView> secondDataList { get; set; } = new ObservableCollection<ImagesView>();
    public static List<Image> firstImageObjectsList = new List<Image>();
    public static List<Image> secondImageObjectsList = new List<Image>();
    public static bool isFirstSelected = false;
    public static bool isSecondSelected = false;
    public static int firstTag;
    public static int secondTag;
    private async void enterBtn_Clicked(object sender, EventArgs e)
    {
        if (isFirstSelected == true && isSecondSelected == true)
        {
            SendUsersTags();
        }
        else
        {
            await DisplayAlert("Внимание", "Выберите по одному изображению из каждой секции!", "ОК");
        }
    }
    public async void LoadImages()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}imagesview/get/random");
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            imagesList = JsonConvert.DeserializeObject<ImagesView[]>(content).ToList();
            foreach (var item in imagesList)
            {
                if (firstDataList.Count < 4)
                {
                    firstDataList.Add(item);
                }
                else
                {
                    secondDataList.Add(item);
                }
            }
            firstImagesCarousel.ItemsSource = firstDataList;
            secondImagesCarousel.ItemsSource = secondDataList;
        }
    }
    public async void SendUsersTags()
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync($"{App.conString}userstags/set/{App.enteredUser.UserId}/{firstTag}/{secondTag}");
        if (response.IsSuccessStatusCode)
        {
            Application.Current.MainPage = new AppShell();
        }
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        LoadImages();
    }

    private void firstImageBorderTap_Tapped(object sender, EventArgs e)
    {
        if (isFirstSelected == false)
        {
            var selectedImage = firstImagesCarousel.CurrentItem as ImagesView;
            firstTag = selectedImage.TagId;
            var currentImage = firstImageObjectsList.Where(x => x.AutomationId == firstTag.ToString()).FirstOrDefault();
            if (currentImage != null)
            {
                currentImage.IsVisible = true;
                isFirstSelected = true;
            }
        }
    }
    private void firstImageBorderDoubleTap_Tapped(object sender, EventArgs e)
    {
        if (isFirstSelected == true)
        {
            var selectedImage = firstImagesCarousel.CurrentItem as ImagesView;
            var currentImage = firstImageObjectsList.Where(x => x.AutomationId == selectedImage.TagId.ToString()).FirstOrDefault();
            if (currentImage != null && currentImage.AutomationId == firstTag.ToString())
            {
                currentImage.IsVisible = false;
                isFirstSelected = false;
                firstTag = 0;
            }
        }
    }

    private void secondImageBorderTap_Tapped(object sender, EventArgs e)
    {
        if (isSecondSelected == false)
        {
            var selectedImage = secondImagesCarousel.CurrentItem as ImagesView;
            secondTag = selectedImage.TagId;
            var currentImage = secondImageObjectsList.Where(x => x.AutomationId == secondTag.ToString()).FirstOrDefault();
            if (currentImage != null)
            {
                currentImage.IsVisible = true;
                isSecondSelected = true;
            }
        }
    }

    private void secondImageBorderDoubleTap_Tapped(object sender, EventArgs e)
    {
        if (isSecondSelected == true)
        {
            var selectedImage = secondImagesCarousel.CurrentItem as ImagesView;
            var currentImage = secondImageObjectsList.Where(x => x.AutomationId == selectedImage.TagId.ToString()).FirstOrDefault();
            if (currentImage != null && currentImage.AutomationId == secondTag.ToString())
            {
                currentImage.IsVisible = false;
                isSecondSelected = false;
                secondTag = 0;
            }
        }
    }


    private void firstCheckImage_Loaded(object sender, EventArgs e)
    {
        Image image = sender as Image;
        var id = image.AutomationId;
        if (firstImageObjectsList.Where(x => x.AutomationId == id).FirstOrDefault() == null)
        {
            firstImageObjectsList.Add(image);
        }
    }
    private void secondCheckImage_Loaded(object sender, EventArgs e)
    {
        Image image = sender as Image;
        var id = image.AutomationId;
        if (secondImageObjectsList.Where(x => x.AutomationId == id).FirstOrDefault() == null)
        {
            secondImageObjectsList.Add(image);
        }

    }
}