using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBProj
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChangePassWord : ContentPage
	{
		public ChangePassWord ()
		{
			InitializeComponent ();
            header.Text = "changing password for " + App.CurrentApp.Globals.loggedUserEmail;
            username.Text = App.CurrentApp.Globals.FirstName + " " + App.CurrentApp.Globals.LastName;
        }
        public void onClickSubmit(object sender, EventArgs e)
        {
            if(newPass.Text != currentPassAgain.Text)
            {
                DisplayAlert("Alert","Password is not the same", "ok");
                return;
            }
            string request = "/api/GetSaltByEmail?email=" + (App.CurrentApp.Globals.loggedUserEmail);
            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            var usersalt = JsonConvert.DeserializeObject<Spice>(json).Salt;

            request = "/api/DoesUserExist?email=" + (App.CurrentApp.Globals.loggedUserEmail) + "&password=";

            SHA256 passSha = SHA256.Create();
            Encoding enc = Encoding.UTF8;
            string passHashed = String.Empty;

            byte[] hashedSequence = passSha.ComputeHash(enc.GetBytes(currentPass.Text + usersalt));
            foreach (byte theByte in hashedSequence)
            {
                passHashed += theByte.ToString("x2");
            }

            string passRehashed = passHashed;

            for(int i = 0; i < App.CurrentApp.hashingTimes; i++)
            {
                passHashed = "";
                hashedSequence = passSha.ComputeHash(enc.GetBytes(passRehashed));
                foreach (byte theByte in hashedSequence)
                {
                    passHashed += theByte.ToString("x2");
                }
                passRehashed = passHashed;
            }

            request = request + passRehashed;

            json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            var ValidPassword = JsonConvert.DeserializeObject<JsonSingleResult>(json).Result;

            if (ValidPassword == 1)
            {
                
                passHashed = String.Empty;

                var guid = Guid.NewGuid();
                var ourSalt = guid.ToString().Replace("-", "");

                hashedSequence = passSha.ComputeHash(enc.GetBytes(newPass.Text + ourSalt));
                foreach (byte theByte in hashedSequence)
                {
                    passHashed += theByte.ToString("x2");
                }

                passRehashed = passHashed;

                for (int i = 0; i < App.CurrentApp.hashingTimes; i++)
                {
                    passHashed = "";
                    hashedSequence = passSha.ComputeHash(enc.GetBytes(passRehashed));
                    foreach (byte theByte in hashedSequence)
                    {
                        passHashed += theByte.ToString("x2");
                    }
                    passRehashed = passHashed;
                }

                request = "api/ChangePasswordByCompIDEmail?compID=" + App.CurrentApp.Globals.compID.ToString() +
                        "&email=" + App.CurrentApp.Globals.loggedUserEmail + "&pass=" + passRehashed + "&salt=" + ourSalt + "&useremail="
                        + App.CurrentApp.Globals.loggedUserEmail;
                json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                var passwordHashed = JsonConvert.DeserializeObject<JsonSingleResult>(json).Result;

                App.NavigationPage.Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Alert", "The password is incorrect for the email", "ok");
            }

           
        }
    }
}