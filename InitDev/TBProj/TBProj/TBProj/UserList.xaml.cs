using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserList : ContentPage
    {
        private string email, status;
        int id;
        public string DisplayEmail { set; get; }
        public string DisplayActive { set; get; }
        public string DisplayID { set; get; }
        bool loading = true, refresh = false;
        int PersonCount = 0;
        int CurrentCount = 0;


        //Collection of UserList
        ObservableCollection<UserList> userEmails = new ObservableCollection<UserList>();
        ObservableCollection<UserList> userDisplay = new ObservableCollection<UserList>();

        public UserList()
        {
            InitializeComponent();
        }

        public UserList(bool load)
        {
            InitializeComponent();
            this.Title = "User List";
            try
            {
                if (App.CurrentApp.Globals.isAdmin)
                {
                    Image.IsVisible = true;
                }
                else
                {
                    Image.IsVisible = false;
                }
                LoadItems();
                nameList.ItemsSource = userDisplay;

                nameList.ItemAppearing += (sender, e) =>
                {
                    if (loading || userDisplay.Count == 0)
                        return;

                    //hit bottom!
                    if (e.Item == userDisplay[userDisplay.Count - 1])
                    {
                        LoadList();
                    }
                };


            }
            catch (Exception e1)
            {
                DisplayAlert("Exception thrown", e1.Message, "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.refresh = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (refresh)
            {
                id = 0;
                CurrentCount = 0;
                userDisplay.Clear();
                PersonCount = 0;
                userEmails.Clear();
                nameList.SelectedItem = null;
                LoadItems();
            }
        }

        public void LoadList()
        {
            loading = true;
            int LoopCount = CurrentCount;
            for (int i = LoopCount; i < (LoopCount + 8); i++)
            {
                if (CurrentCount < PersonCount)
                {
                    CurrentCount++;
                    var x = userEmails[i];
                    userDisplay.Add(x);
                }
                else
                {
                    break;
                }
            }
            nameList.ItemsSource = userDisplay;
            loading = false;
        }

        async void LoadItems()
        {
            try
            {
                var graphrequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.windows.net/technob2c.onmicrosoft.com/users/" + "?api-version=1.6");
                graphrequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.CurrentApp.Globals.Token.AccessToken);

                var Result = await App.CurrentApp.OurGraphClient.SendAsync(graphrequest);
                var content = await Result.Content.ReadAsStringAsync();
                var dez = JsonConvert.DeserializeObject<graphapiList>(content);

                foreach (Value ListItem in dez.Value)
                {
                    this.PersonCount++;

                    userEmails.Add(new UserList { DisplayEmail = ListItem.givenName, DisplayActive = ListItem.accountEnabled.ToString(), DisplayID = ListItem.userPrincipalName });

                }
            }
            catch (Exception e1)
            {
                DisplayAlert("Exception thrown", e1.Message, "OK");
            }

            LoadList();
        }


        private void searchChange(object sender, TextChangedEventArgs e)
        {
            string filter = searchBar.Text;
            nameList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(filter))
            {
                nameList.ItemsSource = userEmails;
            }
            else
            {
                nameList.ItemsSource = userEmails.Where(x => x.DisplayEmail.ToLower().StartsWith(filter.ToLower()));
            }
            nameList.EndRefresh();
        }

        async void onSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selection = e.SelectedItem as UserList;
                //DisplayAlert("You Selected", selection.DisplayEmail,"ok"); 	//Alert for which user has been selected.
                string name = selection.DisplayID;     //sets the var email to the item in UserList

                await App.NavigationPage.Navigation.PushAsync(new EditUser(name));
            }
        }

        async void onClickCreate(object sender, EventArgs e)
        {
            await App.NavigationPage.Navigation.PushAsync(new CreateUser());
        }
    }
}





