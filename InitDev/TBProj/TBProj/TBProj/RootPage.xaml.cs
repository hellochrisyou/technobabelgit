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
    public partial class RootPage : MasterDetailPage //rootpage(masterdetailpage) references the class which is made for implementing hamburger menus
    {

        public RootPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;//displays option for hamburger menu and hides it unless clicked on 
        }

    }
}