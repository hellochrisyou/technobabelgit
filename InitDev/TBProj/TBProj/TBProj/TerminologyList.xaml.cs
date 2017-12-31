

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TerminologyList : ContentPage
    {
        private string name,status;
        bool checkActivate = false;
        bool loading = true,refresh=false;
        bool fromsearchbar = false;
        int id;
        int Page;
        int MaxPage;
        List<TermListItem> ourTermList;
        List<FullTermList> TList;
        List<FullTermList> anothertermlist = new List<FullTermList>();
        public string DisplayTermName { set; get; }//collection of term names
        public string DisplayStatus { set; get; }//collection of term status's
        public Task next { get; private set; }

        ObservableCollection<TerminologyList> termNames = new ObservableCollection<TerminologyList>();//creating collection of term info
        ObservableCollection<TerminologyList> FullList= new ObservableCollection<TerminologyList>();



        public TerminologyList()
        {
            InitializeComponent();
        }

        public TerminologyList(bool load)
        {
            InitializeComponent();

            if(App.CurrentApp.Globals.isAdmin)
            {
                Image.IsVisible = true; 
            }
            else
            {
                Image.IsVisible= false;
            }

            string request = "/api/GetPagesByCompanyIDListName?compID=" + (App.CurrentApp.Globals.compID) + "&listname=termlist";
            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            var max = JsonConvert.DeserializeObject<JsonSingleResult>(json);
            if(max.Result < 2) { MaxPage = 1; } else { MaxPage = max.Result; }

            nameList.ItemsSource = termNames;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (refresh)
            {
                Page = 0;
                id = 0;
                nameList.SelectedItem = null;
                TList.Clear();
                anothertermlist.Clear();
                FullList.Clear();
                termNames.Clear();
                ourTermList.Clear();
                LoadItems();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.refresh=true;
        }

        private async Task LoadItems()
        {
            loading = true;

            if (Page < this.MaxPage)
            {
                Page += 1;
                if (Page == 1)
                {
                    this.Title = "Terminology List";
                    string request = "api/FullTermList?compid=" + App.CurrentApp.Globals.compID.ToString();
                    var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                    var Terms = JsonConvert.DeserializeObject<List<FullTermList>>(json);
                    TList = Terms;
                    anothertermlist.AddRange(Terms);
                    string isActive = "";

                    foreach (FullTermList ListItem in TList)
                    {
                        if (ListItem.Active)
                        {
                            isActive = "Active";
                        }
                        else
                        {
                            isActive = "Not Active";
                        }
                        FullList.Add(new TerminologyList { DisplayTermName = ListItem.TermName, DisplayStatus = isActive });
                        //adds both term names and active status of terms to observable collection list
                    }

                    request = "/api/GetTermListByPageAndCompanyID?page=" + Page + "&compid=" + App.CurrentApp.Globals.compID;
                    json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                    var termList = JsonConvert.DeserializeObject<List<TermListItem>>(json);
                    ourTermList = termList;

                    foreach (TermListItem ListItem in ourTermList)
                    {
                        if (ListItem.Active)
                        {
                            isActive = "Active";
                        }
                        else
                        {
                            isActive = "Not Active";
                        }
                        termNames.Add(new TerminologyList { DisplayTermName = ListItem.TermName, DisplayStatus = isActive });
                        //adds both term names and active status of terms to observable collection list
                    }

                    nameList.ItemsSource = termNames;
                }
                else
                {
                    Device.StartTimer(TimeSpan.FromSeconds(0), () =>
                    {
                        string request = "/api/GetTermListByPageAndCompanyID?page=" + Page + "&compid=" + App.CurrentApp.Globals.compID;
                        var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                        var termList = JsonConvert.DeserializeObject<List<TermListItem>>(json);
                        ourTermList = termList;

                        string isActive = "";

                        foreach (TermListItem ListItem in ourTermList)
                        {
                            if (ListItem.Active)
                            {
                                isActive = "Active";
                            }
                            else
                            {
                                isActive = "Not Active";
                            }
                            termNames.Add(new TerminologyList { DisplayTermName = ListItem.TermName, DisplayStatus = isActive });
                            //adds both term names and active status of terms to observable collection list
                        }

                        nameList.ItemsSource = termNames;
                        return false;
                    });
                }

                loading = false;

            }
        }


        private void searchChange(object sender, TextChangedEventArgs e)
        {

            string filter = searchBar.Text;
            nameList.BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter))
            {
                nameList.ItemsSource = termNames;
            }
            else
            {
                nameList.ItemsSource = FullList.Where(x => x.DisplayTermName.ToLower().StartsWith(filter.ToLower()));
            }
            nameList.EndRefresh();
        }

        private void onSelect(object sender, SelectedItemChangedEventArgs e)
        {
            if (App.CurrentApp.Globals.isAdmin)
            {
                if (e.SelectedItem != null)
                {
                    var selection = e.SelectedItem as TerminologyList;//creates variable selection from the selected term in the obserable list
                                                                      //DisplayAlert("You Selected", selection.DisplayTermName,selection.DisplayStatus, "ok");//displays alert its just neat for now so you can see the implementation. it doesnt need to be here
                    name = selection.DisplayTermName; //let the string variable name = the term name you selected 
                    id = anothertermlist.Where(x => x.TermName == name).First().TermID; //search termNames list for the id associated with the term name selected
                    bool active = anothertermlist.Where(x => x.TermName == name).First().Active;
                    App.NavigationPage.Navigation.PushAsync(new EditTerm(active, name, id));
                }
            }
            else
            {
                if (e.SelectedItem != null)
                {
                    var selection = e.SelectedItem as TerminologyList;//creates variable selection from the selected term in the obserable list
                                                                      //DisplayAlert("You Selected", selection.DisplayTermName,selection.DisplayStatus, "ok");//displays alert its just neat for now so you can see the implementation. it doesnt need to be here
                    name = selection.DisplayTermName; //let the string variable name = the term name you selected 
                    id = anothertermlist.Where(x => x.TermName == name).First().TermID; //search termNames list for the id associated with the term name selected
                    App.NavigationPage.Navigation.PushAsync(new ViewDefinition(id));
                }
            }
        }

        public void onClickCreate(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new CreateTerm());
        }


    }
}


