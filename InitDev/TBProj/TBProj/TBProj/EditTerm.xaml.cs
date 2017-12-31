using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTerm : ContentPage
    {
        bool status;
        int id; 
        string entryTermTmp = "";
        string entryDefinitionTmp = "";
        int entryIdTmp = 0;
        TermListView ourTerm;


        public EditTerm()
        {
            InitializeComponent();
        }

        public EditTerm(bool status,string name, int id)
        {
            InitializeComponent();

            //Id passed in parameters is set to entryIDTmp
            this.entryIdTmp = id;
            this.status = status;
            this.id = id;
            TermName.Text = name;


            //Make API call to get selected terminology definition from ID
            string request = "/api/SelectTermByTermID/" + id;
            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            ourTerm = JsonConvert.DeserializeObject<TermListView>(json);

            Definition.Text = ourTerm.TermDescription;

            privilegesCheck.IsToggled = status;

        }

        public void onClickSubmit(object sender, EventArgs e)
        {
            if (Definition.Text != "")
            {
                //Make API call here
                string request = "/api/UpdateTermByID/" + (entryIdTmp.ToString()) + "?Descript= " + Definition.Text + "&modUser=" + App.CurrentApp.Globals.loggedUserEmail;
                var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                var tempInt = JsonConvert.DeserializeObject<JsonSingleResult>(json);
                if(privilegesCheck.IsToggled!=status)
                {
                    request = "/api/ChangeActivationTermByTermID/" + id + "?email=" + App.CurrentApp.Globals.loggedUserEmail;
                    json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                    var changeResult = JsonConvert.DeserializeObject<JsonSingleResult>(json);
                }

                if (tempInt.Result == -1)
                {

                    DisplayAlert("Alert", "error updating term!", "ok");

                }

                else
                {
                    App.NavigationPage.Navigation.PopAsync();
                    App.MenuIsPresented = false;
                }
            }
        }

    }
}