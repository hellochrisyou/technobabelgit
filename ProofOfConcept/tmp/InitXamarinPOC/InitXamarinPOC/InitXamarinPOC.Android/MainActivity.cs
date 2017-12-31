using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Speech;
using InitXamarinPOC.Droid;
using System.Collections.Generic;

namespace InitXamarinPOC.Droid
{
	[Activity (Label = "InitXamarinPOC", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
        const int tmp = 10; //used to verify request code
        string[] jargon = { "TPS Report", "ACA", "Meeting Minutes", "XML", "Xamarin" }; //List of our terms
        List<string> knownstrings = new List<string>(); //Variable used to keep list of known strings
        

        protected override void OnCreate (Bundle bundle) 
		{


            TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new InitXamarinPOC.App ());
		}

        //caled when voice recorder is done (activity is exiting)
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            knownstrings.Clear(); //Clear list as a new brand new sentence to find the terms 

            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == tmp) //Verifies voice intent
            {
                if (data == null) //Verifies if voice was captures or cancelled
                {
                    return;
                }
                var match = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults); //Retrieve list of text captured by voice recorder
                
                foreach (string resulthitvalue in jargon)
                {
                    if (match[0].ToLower().Contains(resulthitvalue.ToLower())) //
                    {
                        if (!knownstrings.Contains(resulthitvalue))  //Checks if known strings matches with text 
                        {
                            knownstrings.Add(resulthitvalue); //Adds text to known strings
                            SpeechToText.termName = SpeechToText.termName + System.Environment.NewLine + resulthitvalue; //Add terms to stringvariable
                        }
                    }
                }

                if (match.Count != 0) //Checks if text is empty
                    {
                        var textInput = match[0]; 
                        if (textInput.Length > 400) 
                        {
                            textInput = textInput.Substring(0, 400);
                        }
                        SpeechToText.textName = textInput; //Text is added to text string
                    }
                }
                SpeechToText.autoEvent.Set(); //Sets signal to thread
            }
        }

}

