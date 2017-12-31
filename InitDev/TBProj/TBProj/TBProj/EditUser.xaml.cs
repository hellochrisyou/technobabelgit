using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace TBProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUser : ContentPage
    {
        private int id;
        bool isAdmin = false, status, Priv;
        string name, objectId;

        public EditUser(string name)
        {
            InitializeComponent();
            this.name = name;

            Load();

        }
        async void Load()
        {
            var graphrequest = new HttpRequestMessage(HttpMethod.Get, "https://graph.windows.net/technob2c.onmicrosoft.com/users/" + name + "?api-version=1.6");
            graphrequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.CurrentApp.Globals.Token.AccessToken);
            try
            {
                var Result = await App.CurrentApp.OurGraphClient.SendAsync(graphrequest);
                var content = await Result.Content.ReadAsStringAsync();
                var dez = JsonConvert.DeserializeObject<graphapiUser>(content);
                if (dez.employeeId == "ADMIN")
                {
                    PrivilegesCheck.IsToggled = true;
                }
                else
                {
                    PrivilegesCheck.IsToggled = false;
                }
                this.Priv = PrivilegesCheck.IsToggled;
                FirstName.Text = dez.givenName;
                LastName.Text = dez.surname;
                objectId = dez.objectId;
                StatusCheck.IsToggled = dez.accountEnabled;
                onToggled();
            }
            catch (Exception e1)
            {
                DisplayAlert("Alert", "No Connection available", "ok");
                App.NavigationPage.Navigation.PopToRootAsync(false);
            }
        }

        public void focusedentry(object sender, FocusEventArgs fea)
        {
            ExtendedEntry EntrySender = (sender as ExtendedEntry);
            if (EntrySender.Placeholder.Contains("Enter Password"))
            {
                FloatingPassword.Text = "Password";
            }
            if (EntrySender.Placeholder.Contains("Confirm Password"))
            {
                FloatingPasswordConfirm.Text = "Confirm Password";
            }


        }

        public void unfocusedentry(object sender, FocusEventArgs fea)
        {
            ExtendedEntry EntrySender = (sender as ExtendedEntry);
            if (EntrySender.Placeholder.Contains("Enter Password"))
            {
                FloatingPassword.Text = "";
            }
            if (EntrySender.Placeholder.Contains("Confirm Password"))
            {
                FloatingPasswordConfirm.Text = "";
            }
        }

        public void onToggled()
        {
            if (ChangedPassword.IsToggled)
            {
                Password.InputTransparent = false;
                Password.Text = "";
                PasswordConfirm.InputTransparent = false;
                PasswordConfirm.Text = "";
            }
            else
            {
                Password.InputTransparent = true;
                Password.Text = "";
                PasswordConfirm.InputTransparent = true;
                PasswordConfirm.Text = "";
            }
        }
        public async void onClickSubmit(object sender, EventArgs e)
        {
            isAdmin = PrivilegesCheck.IsToggled;
            bool isChangingPassword = ChangedPassword.IsToggled;
            string fname = FirstName.Text;
            string lname = LastName.Text;
            bool status = StatusCheck.IsToggled;
            string privilege;
            string password = Password.Text;
            if (PrivilegesCheck.IsToggled)
            {
                privilege = "ADMIN";
            }
            else
            {
                privilege = "STANDARD";
            }

            //Checks if password matches
            if (isChangingPassword)
            {
                if (Password.Text != PasswordConfirm.Text && Password.Text != null)
                {
                    await DisplayAlert(" ", "Passwords Do Not Match", "ok");
                    return;
                }
                try
                {
                    var json = new JObject
                    {
                        { "surname", lname},
                        {"givenName",fname},
                        { "passwordProfile", new JObject{ {"password",password}, {"forceChangePasswordNextLogin",false}} },
                        { "accountEnabled",false},
                        {"employeeId", privilege}

                    };

                    var jsonObj = JsonConvert.SerializeObject(json);
                    HttpContent graphcontent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var graphrequest = new HttpRequestMessage(new HttpMethod("PATCH"), "https://graph.windows.net/technob2c.onmicrosoft.com/users/" + name + "?api-version=1.6");
                    graphrequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.CurrentApp.Globals.Token.AccessToken);
                    graphrequest.Content = graphcontent;

                    App.CurrentApp.OurGraphClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var Result = await App.CurrentApp.OurGraphClient.SendAsync(graphrequest);
                    var content = await Result.Content.ReadAsStringAsync();
                }
                catch (Exception e1)
                {
                    DisplayAlert("Alert", e1.Message, "ok");
                }
            }
            else
            {
                try
                {
                    var json = new JObject
                    {
                        { "surname", lname},
                        {"givenName",fname},
                        { "accountEnabled",status},
                        {"employeeId", privilege}

                    };

                    var jsonObj = JsonConvert.SerializeObject(json);
                    HttpContent graphcontent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var graphrequest = new HttpRequestMessage(new HttpMethod("PATCH"), "https://graph.windows.net/technob2c.onmicrosoft.com/users/" + name + "?api-version=1.6");
                    graphrequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.CurrentApp.Globals.Token.AccessToken);
                    graphrequest.Content = graphcontent;

                    App.CurrentApp.OurGraphClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var Result = await App.CurrentApp.OurGraphClient.SendAsync(graphrequest);
                    var content = await Result.Content.ReadAsStringAsync();
                }
                catch (Exception e1)
                {
                    DisplayAlert("Alert", e1.Message, "ok");
                }
            }
            if (PrivilegesCheck.IsToggled)
            {
                try
                {
                    var json = new JObject
                    {
                        {
                            "url", "https://graph.windows.net/technob2c.onmicrosoft.com/directoryObjects/"+objectId
                        }
                    };

                    var jsonObj = JsonConvert.SerializeObject(json);
                    HttpContent graphcontent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    var graphrequest = new HttpRequestMessage(new HttpMethod("POST"), "https://graph.windows.net/technob2c.onmicrosoft.com/directoryRoles/b511c079-e57e-48d7-899d-3fa1848b282d/$links/members" + "?api-version=1.6");
                    graphrequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.CurrentApp.Globals.Token.AccessToken);
                    graphrequest.Content = graphcontent;

                    App.CurrentApp.OurGraphClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var Result = await App.CurrentApp.OurGraphClient.SendAsync(graphrequest);
                    var content = await Result.Content.ReadAsStringAsync();
                }
                catch (Exception e1)
                {
                    DisplayAlert("Alert", e1.Message, "ok");
                }

            }
            else
            {
                try
                {

                    var graphrequest = new HttpRequestMessage(new HttpMethod("DELETE"), "https://graph.windows.net/myorganization/directoryRoles/b511c079-e57e-48d7-899d-3fa1848b282d/$links/members/" + objectId + "?api-version" + "?api-version=1.6");
                    graphrequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.CurrentApp.Globals.Token.AccessToken);


                    App.CurrentApp.OurGraphClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var Result = await App.CurrentApp.OurGraphClient.SendAsync(graphrequest);
                    var content = await Result.Content.ReadAsStringAsync();
                }
                catch (Exception e1)
                {
                    DisplayAlert("Alert", e1.Message, "ok");
                }
            }
            await App.NavigationPage.Navigation.PopAsync();
        }
    }
}