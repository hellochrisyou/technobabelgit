using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using System.Threading;

[assembly: Xamarin.Forms.Dependency(typeof(TBProj.iOS.SpeechToText))]
namespace TBProj.iOS
{
    class SpeechToText : SpeechInterface
    {
        public static AutoResetEvent autoEvent = new AutoResetEvent(false); //Sends signal to thread 

        String[] stringDescriptArray = new string[2];
        String[] stringArray;
        static public string textName = "", termName = "";
        const int VOICE = 10; //Used for parameters of intent

        public void toggleSpeech()
        {
            AppDelegate.Self.ListenToggle();
            //((MainActivity)Xamarin.Forms.Forms.Context).ListenToggle();
        }

        public void clearMainActivityHit()
        {
            AppDelegate.Self.clearHits();
            //((MainActivity)Xamarin.Forms.Forms.Context).clearHits();
        }

        public void loginstartcache()
        {
            AppDelegate.Self.createcache();
            //((MainActivity)Xamarin.Forms.Forms.Context).createcache();
        }

        public void sleeplistening()
        {
            AppDelegate.Self.sleeplistener();
           // ((MainActivity)Xamarin.Forms.Forms.Context).sleeplistener();
        }

        public string SpeechToTextAsync()
        {
            stringArray = new string[3];

            autoEvent.Reset();

            stringArray[0] = AppDelegate.Self.ErrorMessage;
            stringArray[1] = AppDelegate.Self.ReturnHits;
            stringArray[2] = AppDelegate.Self.ReturnDescription;
            //stringArray[0] = ((MainActivity)Xamarin.Forms.Forms.Context).ErrorMessage;
            //stringArray[1] = ((MainActivity)Xamarin.Forms.Forms.Context).ReturnHits;
            //stringArray[2] = ((MainActivity)Xamarin.Forms.Forms.Context).ReturnDescription;

            if (stringArray[0].ToString() == "ERROR!!!")
            {
                AppDelegate.Self.ErrorMessage = "";
                //((MainActivity)Xamarin.Forms.Forms.Context).ErrorMessage = "";
                stringDescriptArray[0] = "ERROR!!!";
            }
            else
            {
                stringDescriptArray[0] = stringArray[1];
                stringDescriptArray[1] = stringArray[2];
            }
            return stringDescriptArray[0] + stringDescriptArray[1];
        }
    }
}