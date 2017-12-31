using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Xamarin.Forms;
using XamarinShared;

namespace InitXamarinPOC
{
	public partial class App : Application
	{
        public NavigationPage NavigationPage { get; private set; }
        public App ()
		{
			InitializeComponent();

            var menuPage = new MenuPage();//new instance of menu created 
            NavigationPage = new NavigationPage(new MainPage());//root page added to the navigation stack 
            var rootPage = new RootPage();//new instance rootpage created which references masterdetailpage class
            rootPage.Master = menuPage;//new menu page instance references masterdetailpage.master which dispalys the hamburger menu  
            rootPage.Detail = NavigationPage;//new navigation instance references masterdetailpage.detail(the current page your on)
            MainPage = rootPage;//main page of aplication is now set to rootpage(first page on nav stack) with navigating hamburger menu
                                //.detail references name of the current page
                                //.master lists the pages in the navigation displayed 
                                //rootpage adds pages to the nav stack for masterdetailpage
                                //push a page onto nav stack App.NavigationPage.Navigation.PushAsync(PAGE) 
                                //Or pop a page off the stack(from your MenuPage):App.NavigationPage.Navigation.PopAsync()  
                                
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
