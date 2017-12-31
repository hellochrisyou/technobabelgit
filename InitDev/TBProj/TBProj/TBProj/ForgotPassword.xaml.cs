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
	public partial class ForgotPassword : ContentPage
	{
        public ForgotPassword ()
		{
			InitializeComponent ();
		}

        public void onClickSubmit(object sender, EventArgs e)
        {
            if (entryControl.Text != "")
            {
                string tmp = entryControl.Text;
                tmp = tmp.Replace("@", "%40");
                //DependencyService.Get<SendEmailInterface>().SendEmail(entryControl.Text);
                string request = "/api/ChangeNeedsPassResetByUserEmail?email=" + tmp;
                var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                string check = json;

                if (check == "{\"Result\":1}")
                {
                    confirm.Text = "Sent";
                }
                else
                {
                    confirm.Text = "Error";
                }

            }
        }

    }
}