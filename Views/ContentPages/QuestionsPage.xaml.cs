using Frontend_InkSketch.ApplicationData;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Frontend_InkSketch.Views.ContentPages;

public partial class QuestionsPage : ContentPage
{
    public static Tag[] tagsList = new Tag[] { };
    public static List<UsersTag> dataUsersTag = new List<UsersTag>();
    public static int selectedTags;
    public QuestionsPage()
    {
        InitializeComponent();
    }
    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        //loadingTag.IsRunning = true;
        //IsBusy = true;
        //Label label = (Label)sender;
        //var labelId = int.Parse(label.AutomationId);
        //var selectedTag = tagsList.Where(x => x.TagId == labelId).FirstOrDefault();
        //if (selectedTag != null)
        //{
        //    if (label.TextColor == Colors.Black)
        //    {
        //        if (dataUsersTag.Count == 2)
        //        {
        //            await DisplayAlert("Внимание", "Выберите не более двух тегов!", "Закрыть");
        //        }
        //        else
        //        {
        //            newUsersTag.TagId = selectedTag.TagId;
        //            newUsersTag.UserId = App.enteredUser.UserId;
        //            dataUsersTag.Add(newUsersTag);

        //            label.TextColor = Colors.Green;
        //        }

        //    }
        //    else if (label.TextColor == Colors.Green)
        //    {
        //        var selected = dataUsersTag.Where(x => x.TagId == selectedTag.TagId).FirstOrDefault();
        //        dataUsersTag.Remove(selected);

        //        label.TextColor = Colors.Black;

        //    }
        //}
        //loadingTag.IsRunning = false;
        //IsBusy = false;
        Label label = (Label)sender;
        var labelId = int.Parse(label.AutomationId);
        var selectedTag = tagsList.Where(x => x.TagId == labelId).FirstOrDefault();
        if (selectedTag != null)
        {
            if (label.TextColor == Colors.Black)
            {
                label.TextColor = Colors.Green;
                UsersTag newUsersTag = new UsersTag()
                {
                    TagId = selectedTag.TagId,
                    UserId = App.enteredUser.UserId
                };
                dataUsersTag.Add(newUsersTag);
            }
            else
            {
                label.TextColor = Colors.Black;
                var selectedToRemove = dataUsersTag.Where(x => x.TagId == selectedTag.TagId).FirstOrDefault();
                dataUsersTag.Remove(selectedToRemove);
            }
        }
    }

    private async void tagsCv_Loaded(object sender, EventArgs e)
    {
        try
        {
            HttpClient dataClient = new HttpClient();
            HttpResponseMessage dataResponse = await dataClient.GetAsync($"{App.conString}users/get/data/{App.enteredPhone}");

            if (dataResponse.IsSuccessStatusCode)
            {
                string dataContent = await dataResponse.Content.ReadAsStringAsync();
                var dataUser = JsonConvert.DeserializeObject<User>(dataContent);
                App.enteredUser = dataUser;
            }

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"{App.conString}tags/get");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                tagsList = JsonConvert.DeserializeObject<Tag[]>(content);
                tagsCv.ItemsSource = tagsList.ToList();
            }
        }
        catch (Exception)
        {

        }
    }

    private async void enterBtn_Clicked(object sender, EventArgs e)
    {
        if (dataUsersTag.Count != 0)
        {
            List<HttpStatusCode> responses = new List<HttpStatusCode>();
            foreach (var item in dataUsersTag)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"{App.conString}userstags/set/{App.enteredUser.UserId}/{item.TagId}");
                if (response.IsSuccessStatusCode)
                {
                    responses.Add(response.StatusCode);
                }
            }
            var checkResponses = responses.Contains(HttpStatusCode.BadRequest);
            if (checkResponses)
            {
                await DisplayAlert("Ошибка!", "Ошибка сервера!", "Закрыть");
            }
            else
            {
                Application.Current.MainPage = new AppShell();
            }

        }
        else
        {
            await DisplayAlert("Ошибка", "Выберите хотя бы 1 тег.", "Закрыть");
        }
    }
}