using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TBProj
{
    public partial class MainPage : ContentPage
    {
        string name = "";
        ObservableCollection<bubbleClass> bubbleList = new ObservableCollection<bubbleClass>();
        List<string> stringArray = new List<string>() { "" }; //Receiving string array used for text and term names
        String tmp = "", description;
        Task<String> listeningTask;
        bool listening = false, y = false, buttonclicked = false;
        int index = 0, pageCounter = 1;
        bubbleClass ournewBubble;
        int pages = 1;

        public MainPage(string Email)
        {
            (Application.Current.MainPage as RootPage).IsGestureEnabled = true;



            App.CurrentApp.Globals.loggedUserEmail = Email;
            App.CurrentApp.Globals.lastClickedID = 0;

            InitializeComponent();

            nameList.ItemsSource = bubbleList;
            this.Title = App.CurrentApp.Globals.compName;


            DependencyService.Get<SpeechInterface>().loginstartcache();

            App.CurrentApp.Menu.refresh();

        }


        async void Button_Clicked(object sender, EventArgs e)
        {
            if (buttonclicked) { return; }
            buttonclicked = true;
            if (listening)
            {
                awaitButton.Text = "START LISTENING";
                listening = false;
            }
            else
            {
                awaitButton.Text = "STOP LISTENING";
                listening = true;
            }
            DependencyService.Get<SpeechInterface>().toggleSpeech();
            listeningTask = asyncRecord();
            await listeningTask;
        }



        private void onSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var tmpName = e.SelectedItem as bubbleClass;
            string thisname = tmpName.nameList;
            string ourterm = thisname.Split(':').First();


            string request = "/api/GetTermIDByTermNameCompID?term=" + ourterm + "&compID=" + (App.CurrentApp.Globals.compID.ToString());
            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            var termID = JsonConvert.DeserializeObject<TermIDResult>(json).TermID;

            if (termID > 0)
            {
                (sender as ListView).SelectedItem = null;

                var otherPage = new ViewDefinition(termID);
                App.NavigationPage.Navigation.PushAsync(otherPage);
                App.MenuIsPresented = false;
            }
            else
            {
                (sender as ListView).SelectedItem = null;
                DisplayAlert("Cannot Retrieve Information", "This term no longer exists, it may have been disabled.", "OK");
            }
        }

        async Task<String> asyncRecord()
        {

            bool init = true;

            while (listening == true) //Loop for voice recorder. Cancel voice recorder by clicking outside of voice prompt
            {
                var temp1 = DependencyService.Get<SpeechInterface>().SpeechToTextAsync();
                string strippedtemp1 = temp1;
                strippedtemp1 = strippedtemp1.Replace(':', ' ');
                strippedtemp1 = strippedtemp1.Trim();

                if (temp1 == "ERROR!!!")
                {
                    if (!init)
                    {
                        listening = false;
                        awaitButton.Text = "START LISTENING";
                    }
                }
                else if (temp1 != stringArray[index] && strippedtemp1 != "")
                {

                    pageCounter = 1;
                    ournewBubble = new bubbleClass { nameList = temp1 };

                    bubbleList.Add(ournewBubble);
                    nameList.ItemsSource = bubbleList;
                    nameList.ScrollTo(ournewBubble, ScrollToPosition.End, true);
                    stringArray.Add(temp1);
                    index++;

                    string request = "/api/IncreaseListenCountByCompIDTermName/" + (App.CurrentApp.Globals.compID.ToString()) + "?Term=" + temp1.Split(':').First();
                    var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                    var increaseResult = JsonConvert.DeserializeObject<JsonSingleResult>(json).Result;

                    DependencyService.Get<SpeechInterface>().clearMainActivityHit();
                }
                //stringArray =  //dependency injection
                //set text via referencing name
                //textName.Text = stringArray[1]

                await Task.Delay(200);   //two second delay
                init = false;
                buttonclicked = false;
            }
            return tmp;
        }

    }
}