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
    public partial class MenuPage : ContentPage //menupage is used to display the hamburger menu
    {

        public MenuPage()
        {
            BindingContext = new MenuView();
            Title = "Menu";
            InitializeComponent();

        }
        public void refresh()
        {
            if (!(App.CurrentApp.Globals.isAdmin))
            {
                userlist.IsVisible = false;
            }
            else
            {
                userlist.IsVisible = true;
            }
            useremail.Text = App.CurrentApp.Globals.loggedUserEmail;
            username.Text = App.CurrentApp.Globals.FirstName + "  " + App.CurrentApp.Globals.LastName;

        }
    }
}