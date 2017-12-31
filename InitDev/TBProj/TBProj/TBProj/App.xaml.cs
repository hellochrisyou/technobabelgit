using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TBProj
{
	public partial class App : Application
	{
        public static RootPage rootPage;//creating reference to rootPage used for below bool variable rootPage set in Login function
        public static bool MenuIsPresented//bool public used variable to hide ham menu when clicking on a page
        {
            get
            {
                return rootPage.IsPresented; //gets current set bool which is true
            }
            set
            {
                rootPage.IsPresented = value; //sets value either true(open) or false(close)
            }

        }

        public int hashingTimes { get; set; }

        public static NavigationPage NavigationPage { get; private set; }

        public static App CurrentApp { get; set; }

        public MenuPage Menu { get; set; }

        public HttpClient OurClient { get; set; }

        public HttpClient OurGraphClient { get; set; }

        public GlobalVariablesApp Globals { get; set; }

        //initialize application
        public App()
        {
            Globals = new GlobalVariablesApp();

            InitializeComponent();
            MainPage = new Splash();
            CurrentApp = this;
            CurrentApp.hashingTimes = 19;
            loadingSplash();
        }

        //Connect to Azure while loading splash screen for 2 seconds
        async Task loadingSplash()
        {
            OurClient = new HttpClient();
            OurClient.BaseAddress = new
                    Uri("https://apitechnobabel.azurewebsites.net");

            OurGraphClient = new HttpClient();

            //Statement to open up the connection.
            OurClient.GetStringAsync("/api/GetSaltByEmail?email=x");

            await Task.Delay(2000);

            Login(); 
        }

        public void Login()
        {
            NavigationPage = new NavigationPage(new Login());//main page added to the navigation stack 
            var menuPage = new MenuPage();
            Menu = menuPage;
            rootPage = new RootPage();//new instance rootpage created which references masterdetailpage class
            rootPage.Master = menuPage;//new menu page instance references masterdetailpage.master which dispalys the hamburger menu list  
            rootPage.Detail = NavigationPage;//new navigation instance references masterdetailpage.detail(the current page your on)
            MainPage = rootPage;//main page of aplication is now set to rootpage(first page on nav stack) with navigating hamburger menu
                                //.detail references name of the current page
                                //.master lists the pages in the navigation displayed 
                                //push a page onto nav stack App.NavigationPage.Navigation.PushAsync(PAGE) 
                                //Or pop a page off the stack(from your MenuPage):App.NavigationPage.Navigation.PopAsync() 
            (Application.Current.MainPage as RootPage).IsGestureEnabled = false;
            rootPage.Master.BackgroundColor = Color.White;
            NavigationPage.BarTextColor = Color.FromHex("#FAFAFA");
            NavigationPage.BarBackgroundColor = Color.FromHex("#212121");
        }
       
        protected override void OnStart ()
		{
			// Handle when your app starts
		}

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            DependencyService.Get<SpeechInterface>().sleeplistening();
        }

=======
		protected override void OnSleep ()
		{
            // Handle when your app sleeps

            DependencyService.Get<SpeechInterface>().sleeplistening();
        }
>>>>>>> 5f82d9e8cb30382f50da3f12b6185c7e98b8f5a9

        protected override void OnResume ()
		{
			// Handle when your app resumes
            if(App.CurrentApp.Globals.compID > 0)
            { 
                DependencyService.Get<SpeechInterface>().loginstartcache();
            }
		}
	}
}
