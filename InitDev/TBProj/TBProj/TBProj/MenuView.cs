using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TBProj
{
    public class MenuView : ContentPage
    {
        public ICommand MainMenu { get; set; }//recognizing binding for MainMenu
        public ICommand Help { get; set; }
        public ICommand UserList { get; set; }
        public ICommand TermList { get; set; }
        public ICommand View { get; set; }
        public ICommand HitCount { get; set; }
        public ICommand Logout { get; set; }
        public ICommand AdminList { get; set; }

        public MenuView()
        {
            MainMenu = new Command(MenuDisplay);//creating command for MainMenu and calling the Menu function to navigate
            Help = new Command(HelpDisplay);
            UserList= new Command(UserDisplay);
            TermList= new Command(TermDisplay);
            View = new Command(ViewDisplay);
            HitCount = new Command(HitCountDisplay);
            Logout = new Command(LogoutDisplay);
            AdminList = new Command(AdminDisplay);
        }
        public string Name { set; get; }
        void MenuDisplay(object obj)
        {
            var otherPage = new MainPage(App.CurrentApp.Globals.loggedUserEmail);
            var homePage = App.NavigationPage.Navigation.NavigationStack.First();
            App.NavigationPage.Navigation.InsertPageBefore(otherPage, homePage);
            App.NavigationPage.PopToRootAsync(false);
            App.MenuIsPresented = false; //closes the ham menu
        }

        async void HelpDisplay(object obj)
        {
            await App.NavigationPage.Navigation.PushAsync(new HelpMenu());
            App.MenuIsPresented = false;
        }
        async void UserDisplay(object obj)
        {
            await App.NavigationPage.Navigation.PushAsync(new UserList(true));
            App.MenuIsPresented = false;
        }

        async void TermDisplay(object obj)
        {
            await App.NavigationPage.Navigation.PushAsync(new TerminologyList(true));
            App.MenuIsPresented = false;
        }
        async void ViewDisplay(object obj)
        {
            if(App.CurrentApp.Globals.lastClickedID == 0) { DisplayAlert("No Last Term", "You have not clicked on a term.", "Ok"); return; }
            await App.NavigationPage.Navigation.PushAsync(new ViewDefinition(App.CurrentApp.Globals.lastClickedID));
            App.MenuIsPresented = false;
        }

        async void HitCountDisplay(object obj)
        {
            await App.NavigationPage.Navigation.PushAsync(new Hit_Count(true));
            App.MenuIsPresented = false;
        }
        async void AdminDisplay(object obj)
        {
            await App.NavigationPage.Navigation.PushAsync(new AdministrateScreen());
            App.MenuIsPresented = false;
        }

        void LogoutDisplay(object obj)
        {
            var clearToken=DependencyService.Get<IAuthenticator>();
            clearToken.Logout();
            App.CurrentApp.Globals.ResetGlobals();
            var otherPage = new Login();
            var homePage = App.NavigationPage.Navigation.NavigationStack.First();
            App.NavigationPage.Navigation.InsertPageBefore(otherPage, homePage);
            App.NavigationPage.PopToRootAsync(false);
            App.MenuIsPresented = false; //closes the ham menu
        }

    }
}
