using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace TBProj
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateUser : ContentPage
    {
        string email;
        bool privileges;

        public CreateUser()
        {
            InitializeComponent();
        }

        //Create user using entry fields
        public async void onClickSubmit(object sender, EventArgs e)
        {
            string fname = firstName.Text;
            string lname = lastName.Text;
            string privilege;
            string password = passwordName.Text;
            email = emailAddressName.Text;
            privileges = privilegesCheck.IsToggled;

            RegEx regexcheck = new RegEx();

            if (regexcheck.IsValidEmail(email))
            {
                // DisplayAlert("Alert", "Email valid", "ok");
            }
            else
            {
                DisplayAlert("Alert", "Email not valid", "ok");
                return;
            }

            if (string.IsNullOrEmpty(firstName.Text) == true || string.IsNullOrEmpty(lastName.Text) == true || string.IsNullOrEmpty(emailAddressName.Text) == true || string.IsNullOrEmpty(passwordName.Text) == true || string.IsNullOrEmpty(passwordConfirm.Text) == true)
            {
                DisplayAlert("Alert", "Entry fields cannot be empty", "ok");
                return;
            }

            if (passwordName.Text != passwordConfirm.Text)
            {
                DisplayAlert("Alert", "Passwords do not match", "ok");
                return;
            }
            if (privileges)
            {
                privilege = "ADMIN";
            }
            else
            {
                privilege = "STANDARD";
            }
            try
            {
                var json = new JObject
                    {
                        { "surname", lname},
                        {"givenName",fname},
                        { "passwordProfile", new JObject{ {"password",password}, {"forceChangePasswordNextLogin",false}} },
                        { "accountEnabled",true},
                        {"employeeId", privilege},
                    {"department",App.CurrentApp.Globals.compName },
                    {"userPrincipalName",email},
                    {"displayName",fname+" "+lname },
                    {"mailNickname", fname }

                    };

                var jsonObj = JsonConvert.SerializeObject(json);
                HttpContent graphcontent = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                var graphrequest = new HttpRequestMessage(new HttpMethod("POST"), "https://graph.windows.net/technob2c.onmicrosoft.com/users/" + "?api-version=1.6");
                graphrequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", App.CurrentApp.Globals.Token.AccessToken);
                graphrequest.Content = graphcontent;

                App.CurrentApp.OurGraphClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var Result = await App.CurrentApp.OurGraphClient.SendAsync(graphrequest);
                var content = await Result.Content.ReadAsStringAsync();
                var dez = JsonConvert.DeserializeObject<JsonSingleResult>(content);

            }
            catch (Exception e1)
            {
                DisplayAlert("Alert", e1.Message, "ok");
            }
            await App.NavigationPage.Navigation.PopAsync();
        }

        public void focusedentry(object sender, FocusEventArgs fea)
        {
            ExtendedEntry EntrySender = (sender as ExtendedEntry);
            if (EntrySender.Placeholder.Contains("First Name"))
            {
                FloatingFirstName.Text = "First Name";
            }
            if (EntrySender.Placeholder.Contains("Last Name"))
            {
                FloatingLastName.Text = "Last Name";
            }
            if (EntrySender.Placeholder.Contains("Email"))
            {
                FloatingEmail.Text = "Email";
            }
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

            if (EntrySender.Placeholder.Contains("First Name"))
            {
                FloatingFirstName.Text = "";
            }
            if (EntrySender.Placeholder.Contains("Last Name"))
            {
                FloatingLastName.Text = "";
            }
            if (EntrySender.Placeholder.Contains("Email"))
            {
                FloatingEmail.Text = "";
            }
            if (EntrySender.Placeholder.Contains("Enter Password"))
            {
                FloatingPassword.Text = "";
            }
            if (EntrySender.Placeholder.Contains("Confirm Password"))
            {
                FloatingPasswordConfirm.Text = "";
            }
        }
    }
}