
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace TBProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateTerm : ContentPage
    {
        string termTmp = "";
        string definitionTmp = "";

        public CreateTerm()
        {
            InitializeComponent();
        }

        public void focusededitor(object sender, FocusEventArgs fea)
        {
            FloatingDefinition.Text = "Term Definition";
        }

        public void unfocusededitor(object sender, FocusEventArgs fea)
        {
            FloatingDefinition.Text = "";
        }


        public void focusedentry(object sender, FocusEventArgs fea)
        {
            ExtendedEntry EntrySender = (sender as ExtendedEntry);
            if (EntrySender.Placeholder.Contains("Enter Term Name"))
            {
                FloatingTermName.Text = "Term Name";
            }
        }

        public void unfocusedentry(object sender, FocusEventArgs fea)
        {
            ExtendedEntry EntrySender = (sender as ExtendedEntry);
            if (EntrySender.Placeholder.Contains("Enter Term Name"))
            {
                FloatingTermName.Text = "";
            }
        }

    //Create terminology from entry fields
    public void onClickSubmit(object sender, EventArgs e)
        {
            termTmp = entryName.Text;
            definitionTmp = entryDefinition.Text;

            if (termTmp != "" && definitionTmp != "")
            {
                //Make API call here
                string request = "/api/InsertTerm?createUser=" + App.CurrentApp.Globals.loggedUserEmail + "&comID=" + (App.CurrentApp.Globals.compID.ToString()) + "&Term=" + termTmp + "&Description=" + definitionTmp;
                var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                var tempInt = JsonConvert.DeserializeObject<JsonSingleResult>(json);
             
                if (tempInt.Result == -1)
                {
                    DisplayAlert(" ", "Term Already Exists!", "ok");
                    return; 
                }
                //Pop page off and return to terminology list
                else
                {
                    App.NavigationPage.Navigation.PopAsync();
                    App.MenuIsPresented = false;
                }
            }
        }
    }
}
