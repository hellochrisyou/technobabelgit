using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InitXamarinPOC
{

    public partial class MainPage : ContentPage
    {
        String[] stringArray = new string[2]; //Receiving string array used for text and term names
        String tmp = "";
        bool check = false;
        CancellationTokenSource cts = null;

        public MainPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
                await asyncRecord();
        }

       async Task<String> asyncRecord()
        {
            while (check == false) //Loop for voice recorder. Cancel voice recorder by clicking outside of voice prompt
            {
                stringArray= await DependencyService.Get<Interface1>().SpeechToTextAsync(); //dependency injection
                termName.Text = stringArray[0]; //set text via referencing name
                textName.Text = stringArray[1];
                await Task.Delay(2000);   //two second delay            
            }
            return tmp;
        }

    }

}


