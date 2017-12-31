//class that runs dependency injection
using System;

using Android.App;
using Android.Content;

using System.Threading.Tasks;
using Android.Speech;
using Xamarin.Forms;
using System.Threading;
using InitXamarinPOC.Droid;

//need assembly reference for Dependency
[assembly: Xamarin.Forms.Dependency(typeof(InitXamarinPOC.Droid.SpeechToText))]
namespace InitXamarinPOC.Droid
{
    class SpeechToText : Interface1
    {
        String[] stringArray = new string[2]; //String array used for text and term names
        static public string textName = "", termName = "";
        public static AutoResetEvent autoEvent = new AutoResetEvent(false); //Sends signal to thread 
        const int VOICE = 10; //Used for parameters of intent

        public async Task<string[]> SpeechToTextAsync()
        {
            //Initialize voice intent to be passed when starting activity
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 2000); //Waits for 2 seconds before ending voice recorder
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 2000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 2000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

            autoEvent.Reset(); 
            ((MainActivity)Forms.Context).StartActivityForResult(voiceIntent, VOICE); //Start main activity with speech intent
            await Task.Run(() => { autoEvent.WaitOne(new TimeSpan(0, 2, 0)); }); //Thread waits for two minutes
            stringArray[0] = termName; 
            stringArray[1] = textName;
            return stringArray;
        }



    }

    

}