using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewDefinition : ContentPage
    {
        TermListView ourTerm;
        int id; 
        
     
        public ViewDefinition(int id)
        {
            InitializeComponent();
            this.id = id;
            //Make API call here
            string request = "/api/SelectTermByTermID/" + id;
            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            ourTerm = JsonConvert.DeserializeObject<TermListView>(json);

            termVar.Text = ourTerm.TermName;
            definitionVar.Text = ourTerm.TermDescription;

            request = "/api/IncreaseClickCountByCompIDTermName/" + App.CurrentApp.Globals.compID.ToString() + "?Term=" + ourTerm.TermName;
            json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            var response = JsonConvert.DeserializeObject<JsonSingleResult>(json);
            App.CurrentApp.Globals.lastClickedID = id;

        }
  

    }
}
