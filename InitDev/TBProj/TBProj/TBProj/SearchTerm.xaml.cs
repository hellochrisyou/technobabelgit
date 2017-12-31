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
	public partial class SearchTerm : ContentPage
	{
        string name = "";
        List<string> terms = new List<string> { "TPS Report", "Test", "Meeting Minutes", "XML", "Xamarin", "Ben", "Eli", "Chris", "Joe" };

        public SearchTerm ()
		{
			InitializeComponent();

            nameList.ItemsSource = terms;
        }

        private void searchChange(object sender, TextChangedEventArgs e)
        {
            var word = searchBar.Text;
            if (word.Length > 2)
            {
                nameList.ItemsSource = terms.Where(name => name.ToLower().Contains(word.ToLower()));
            }
            else
            {
                nameList.ItemsSource = terms;
            }
        }

        private void onSelect(object sender, SelectedItemChangedEventArgs e)
        {
            name = e.SelectedItem.ToString();

        }

        public void onClickView(object sender, EventArgs e)
        {
            //var otherPage = new ViewDefinition(name);
            //App.CurrentApp.NavigationPage.PushAsync(otherPage);

            /*
            var homePage = App.NavigationPage.Navigation.NavigationStack.First();
            App.NavigationPage.Navigation.InsertPageBefore(otherPage, homePage);
            App.NavigationPage.PopToRootAsync(false);*/

            //Navigation.PushModalAsync(new ViewDefinition());
        }

        public void onClickEdit(object sender, EventArgs e)
        {
            //var otherPage = new EditTerm();
            //App.CurrentApp.NavigationPage.PushAsync(otherPage);
            /*
            var homePage = App.NavigationPage.Navigation.NavigationStack.First();
            App.NavigationPage.Navigation.InsertPageBefore(otherPage, homePage);
            App.NavigationPage.PopToRootAsync(false);*/

            //Navigation.PushModalAsync(new EditTerm());
        }

        public void onClickRemove(object sender, EventArgs e)
        {
            //Connect to Database
        }

        public void onClickCreate(object sender, EventArgs e)
        {
            //var otherPage = new CreateTerm();
            //App.CurrentApp.NavigationPage.PushAsync(otherPage);
            /*
            var homePage = App.NavigationPage.Navigation.NavigationStack.First();
            App.NavigationPage.Navigation.InsertPageBefore(otherPage, homePage);
            App.NavigationPage.PopToRootAsync(false);*/

            //Navigation.PushModalAsync(new CreateTerm());
        }
    }
}