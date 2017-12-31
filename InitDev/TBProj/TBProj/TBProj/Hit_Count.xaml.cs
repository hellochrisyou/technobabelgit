using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace TBProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Hit_Count : ContentPage
    {
        bool loading = true;
        int MaxPage;
        public int page = 0;
        public bool heard;
        List<topTenTerm> termList;
        public string DisplayTerm { set; get; }
        public int DisplayHits { set; get; }
        public int DisplayClicks { set; get; }
        ObservableCollection<Hit_Count> termNames = new ObservableCollection<Hit_Count>();

        public Hit_Count()
        {
            this.Title = "Frequency Count";
            InitializeComponent();
        }

        public Hit_Count(bool Listened)
        {
            this.Title = "Frequency Count";
            InitializeComponent();
            this.heard = Listened;

            //Make API call to get list of Terms
            string request = "/api/GetPagesByCompanyIDListName?compID=" + (App.CurrentApp.Globals.compID) + "&listname=termlist";
            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            var max = JsonConvert.DeserializeObject<JsonSingleResult>(json);
            if (max.Result < 2) { this.MaxPage = 1; } else { this.MaxPage = max.Result; }

            nameList.ItemsSource = termNames;

            //Infinite scroll list
            nameList.ItemAppearing += (sender, e) =>
            {
                if (loading || termNames.Count == 0)
                    return;

                //hit bottom!
                if (e.Item == termNames[termNames.Count - 1])
                {
                    LoadItems();
                }
            };
            LoadItems();
        }

        public void LoadItems()
        {
            loading = true;
            if (page < this.MaxPage)
            {
                page += 1;
                if (page == 1)
                {
                    //Make API call to get frequency list
                    string request = "api/GetFrequencyListByPageAndCompanyID?page=" + page + "&compid=" + App.CurrentApp.Globals.compID.ToString() + "&listened=" + heard;
                    var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                    termList = JsonConvert.DeserializeObject<List<topTenTerm>>(json);
                    foreach (topTenTerm ttt in termList)
                    {
                        termNames.Add(new Hit_Count { DisplayTerm = ttt.TermName, DisplayClicks = ttt.ClickCount, DisplayHits = ttt.ListenCount });

                    }

                    //set list into bindable name list
                    nameList.ItemsSource = termNames;
                }
                else
                {
                    //Infinite scroll list 
                    Device.StartTimer(TimeSpan.FromSeconds(0), () =>
                    {
                        //Make API call to get frequency list after page 1
                        string request = "api/GetFrequencyListByPageAndCompanyID?page=" + page + "&compid=" + App.CurrentApp.Globals.compID.ToString() + "&listened=" + heard;
                        var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                        termList = JsonConvert.DeserializeObject<List<topTenTerm>>(json);
                        foreach (topTenTerm ttt in termList)
                        {
                            termNames.Add(new Hit_Count { DisplayTerm = ttt.TermName, DisplayClicks = ttt.ClickCount, DisplayHits = ttt.ListenCount });

                        }
                        nameList.ItemsSource = termNames;
                        return false;
                    });
                }
                //Loading infinite scroll list stops
                loading = false;
            }
        }

        //Selecting item
        private void onSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            (sender as ListView).SelectedItem = null;
        }

        //Switches to listen count
        public void TapListenCount(object send, EventArgs e)
        {
            heard = true;
            termNames.Clear();
            page = 0;
            LoadItems();
        }

        //Switches to click count
        public void TapClickCount(object send, EventArgs e)
        {
            heard = false;
            termNames.Clear();
            page = 0;
            LoadItems();
        }
    }
}