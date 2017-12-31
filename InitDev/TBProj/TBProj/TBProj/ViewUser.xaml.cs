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
	public partial class ViewUser : ContentPage
	{
        bool loading;

        public ViewUser(int id)
        {
            InitializeComponent();
            //Make the call to the API to get the user's info

            loading = true;

            string request = "/api/SelectUserByUserID?usersID=" + id;
            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            var userinfo = JsonConvert.DeserializeObject<UserListView>(json);

            firstName.Text = userinfo.FirstName;
            lastName.Text = userinfo.LastName;
            userEmail.Text = userinfo.Email;
            isAdminSwitch.IsToggled = userinfo.isAdmin;

            loading = false;
        }

        public void DeselectEditor(object sender, FocusEventArgs e)
        {
            if (sender is Editor)
            {
                (sender as Editor).Unfocus();
            }
            if (sender is Entry)
            {
                (sender as Entry).Unfocus();
            }
        }

        public void DisableToggle(object sender, ToggledEventArgs e)
        {
            if (loading)
            {
                loading = false;
                return;
            }
            loading = true;
            Switch thisSender = (Switch)sender;
            thisSender.IsToggled = !thisSender.IsToggled;
        }
    }
}