//class that runs dependency injection
using System;

using Android.App;
using Android.Content;

using System.Threading.Tasks;
using Android.Speech;
using Xamarin.Forms;
using System.Threading;


[assembly: Xamarin.Forms.Dependency(typeof(TBProj.Droid.SpeechToText))]
namespace TBProj.Droid
{
    class SpeechToText : SpeechInterface
    {
        String[] stringArray = new string[2]; //String array used for text and term names
        static public string textName = "", termName = "";
        public static AutoResetEvent autoEvent = new AutoResetEvent(false); //Sends signal to thread 
        const int VOICE = 10; //Used for parameters of intent
        string stringTmp;


        public void toggleSpeech()
        {
            ((MainActivity)Xamarin.Forms.Forms.Context).ListenToggle();
        }

        public void clearMainActivityHit()
        {
            ((MainActivity)Xamarin.Forms.Forms.Context).clearHits();
        }

        public void loginstartcache()
        {
            ((MainActivity)Xamarin.Forms.Forms.Context).createcache();

        }
        public void sleeplistening()
        {
            ((MainActivity)Xamarin.Forms.Forms.Context).sleeplistener();
        }

<<<<<<< HEAD

        public void sleeplistening()
        {
            ((MainActivity)Xamarin.Forms.Forms.Context).sleeplistener();
        }


        public string SpeechToTextAsync()
=======
    public string SpeechToTextAsync()
>>>>>>> 5f82d9e8cb30382f50da3f12b6185c7e98b8f5a9
        {
            stringTmp = "";
            stringArray = new string[2];
            /*
            //Initialize voice intent to be passed when starting activity
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 2000); //Waits for 2 seconds before ending voice recorder
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 2000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 2000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);*/

            autoEvent.Reset();

            //((MainActivity)Forms.Context).StartActivityForResult(voiceIntent, VOICE); //Start main activity with speech intent
            //await Task.Run(() => { autoEvent.WaitOne(new TimeSpan(0, 2, 0)); }); //Thread waits for two minutes

            stringArray[0] = ((MainActivity)Xamarin.Forms.Forms.Context).ErrorMessage;
            stringArray[1] = ((MainActivity)Xamarin.Forms.Forms.Context).ReturnHits;

            if (stringArray[0].ToString() == "ERROR!!!")
            {
                ((MainActivity)Xamarin.Forms.Forms.Context).ErrorMessage = "";
                stringTmp = "ERROR!!!";
            }
            else
            {
                stringTmp = stringArray[1];
            }

            return stringTmp;
        }   



    }

}




