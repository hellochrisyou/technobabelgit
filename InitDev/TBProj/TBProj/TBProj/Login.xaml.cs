using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace TBProj
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        private async void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (!e.IsConnected)
            {
                await DisplayAlert("Error", "Connection to the internet has been lost.", "ok");
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "No internet connection found.", "ok");
            }
        }


        public void onClickSubmit(object sender, EventArgs e)
        {
            var clearToken = DependencyService.Get<IAuthenticator>();
            clearToken.Logout();

            Task T = logintopage();
        }

        public async Task logintopage()
        {


            /*

                string request = "/api/GetSaltByEmail?email=" + username.Text;
                var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                var usersalt = JsonConvert.DeserializeObject<Spice>(json).Salt;

                request = "/api/DoesUserExist?email=" + username.Text + "&password=";

                //Hashing
                SHA256 passSha = SHA256.Create();
                Encoding enc = Encoding.UTF8;
                string passHashed = String.Empty;


                byte[] hashedSequence = passSha.ComputeHash(enc.GetBytes(password.Text + usersalt));

                foreach (byte theByte in hashedSequence)
                {
                    passHashed += theByte.ToString("x2");
                }


            SHA256 passSha = SHA256.Create();
            Encoding enc = Encoding.UTF8;
            string passHashed = String.Empty;


            byte[] hashedSequence = passSha.ComputeHash(enc.GetBytes(password.Text + usersalt));
             foreach (byte theByte in hashedSequence)
            {
                passHashed += theByte.ToString("x2");
            }

            string passRehashed = passHashed;

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

            request = request + passRehashed;

            json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            var loginValid = JsonConvert.DeserializeObject<JsonSingleResult>(json).Result;*/

            string clientId = "acfd2c6c-40b9-4dbd-89e0-b7be0e2d306a";
            string authority = "https://login.windows.net/technob2c.onmicrosoft.com";
            string returnUri = "https://apitechnobabel.azurewebsites.net/.auth/login/aad/callback";
            string resource = "9547880c-43a9-468f-8aa4-061335a6fa7f";
            AuthenticationResult authResult = null;
                var auth = DependencyService.Get<IAuthenticator>();

                AuthenticationResult graphdata = await auth.Authenticate("https://login.windows.net/common", "https://graph.windows.net", clientId, returnUri);


                AuthenticationResult data = await auth.Authenticate(authority, resource, clientId, returnUri);
                App.CurrentApp.OurClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", data.AccessToken);

                
                var graphrequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.windows.net/technob2c.onmicrosoft.com/users/" + graphdata.UserInfo.DisplayableId + "?api-version=1.6");
                graphrequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", graphdata.AccessToken);
                //var serializedobject = JsonConvert.SerializeObject(new { securityEnabledOnly = false });
                //var graphcontent = new System.Net.Http.StringContent(serializedobject, Encoding.UTF8, "application/json");
                //graphrequest.Content = graphcontent;

                var Result = await App.CurrentApp.OurGraphClient.SendAsync(graphrequest);
                var content = await Result.Content.ReadAsStringAsync();
                var dez = JsonConvert.DeserializeObject<graphapiUser>(content);

                if (!String.IsNullOrEmpty(dez.department))
                {

                    string request = "/api/GetCompanyIDByCompanyName?compname=" + dez.department;
                    var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                    var compIDRESULT = JsonConvert.DeserializeObject<SingleCompID>(json).CompanyID;

                    if (compIDRESULT > 0)
                    {
                        if (dez.employeeId.Equals("ADMIN"))
                        {
                        App.CurrentApp.Globals.isAdmin = true;
                        }
                        App.CurrentApp.Globals.compID = compIDRESULT;
                        App.CurrentApp.Globals.compName = dez.department;
                        App.CurrentApp.Globals.FirstName = dez.givenName;
                        App.CurrentApp.Globals.LastName = dez.surname;
                        App.CurrentApp.Globals.Token = graphdata;

                        var otherPage = new MainPage(graphdata.UserInfo.DisplayableId);
                        var homePage = App.NavigationPage.Navigation.NavigationStack.First();
                        App.NavigationPage.Navigation.InsertPageBefore(otherPage, homePage);
                        await App.NavigationPage.PopToRootAsync(false);
                    }
                    else
                    {
                        DisplayAlert("Login Error", "Company Information Invalid", "OK");
                    }
                }
                else
                {
                    DisplayAlert("Login Error", "Invalid Username or Password", "OK");
                    }
                    }
            

        public void onClickForgotPassword(object sender, EventArgs e)
        {
            App.NavigationPage.Navigation.PushAsync(new ForgotPassword());

        }

        /*
        public void ShowEntryForUsername(object sender, EventArgs e)
        {
            FloatingUsername.Text = "USERNAME";
        }

        public void ShowEntryForPassword(object sender, EventArgs e)
        {
            FloatingPassword.Text = "PASSWORD";
        }

        public void HideEntryForUsername(object sender, EventArgs e)
        {
            FloatingUsername.Text = "";
        }

        public void HideEntryForPassword(object sender, EventArgs e)
        {
            FloatingPassword.Text = "";
        }
        */
    }
}