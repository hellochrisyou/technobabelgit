using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinShared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage //menupage is used to display the hamburger menu
    {
        public MenuPage()
        {
            Title = "Menu";
            InitializeComponent();
        }
    }
}