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
	public partial class AdministrateScreen : ContentPage
    {

        List<AdministrateListItem> userAdminList;
        public string DisplayEmail { set; get; }
        public string DisplayLocked { set; get; }
        public string DisplayPassword { set; get; }
        ObservableCollection<AdministrateScreen> userNames = new ObservableCollection<AdministrateScreen>();

        public AdministrateScreen ()
		{
            /*
			InitializeComponent ();
            string request = "/api/GetAdminListByPageAndCompanyID?page=1&compID=" + App.CurrentApp.Globals.compID.ToString();
            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            userAdminList = JsonConvert.DeserializeObject<List<AdministrateListItem>>(json);

            string tempLocked = "";
            string tempPass = "";
            foreach (AdministrateListItem ALI in userAdminList)
            {
                if (ALI.Locked) { tempLocked = "Yes";}
                else { tempLocked = "No"; }

                if (ALI.NeedsPasswordReset) { tempPass = "Yes"; }
                else { tempPass = "No"; }
                userNames.Add(new AdministrateScreen { DisplayEmail = ALI.Email, DisplayLocked = tempLocked, DisplayPassword = tempPass });

            }
            nameList.ItemsSource = userNames;
            */
        }
	}
}
