using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TBProj
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Splash : ContentPage
	{
		public Splash ()
		{
			InitializeComponent();

            /*
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new
                    Uri("https://apitechnobabel.azurewebsites.net");

                var json = client.GetStringAsync("/").Result;
                var contacts = JsonConvert.DeserializeObject<contact[]>(json);
            }
            */
        }
	}
}