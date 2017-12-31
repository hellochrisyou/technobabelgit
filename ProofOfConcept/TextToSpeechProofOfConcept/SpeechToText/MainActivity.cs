//Please note that the application must have RECORD_AUDIO permission to use this class. 

using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Widget;
using Android.OS;
using Android.Speech;
using Android.Util;
using Android;
using Android.Content.PM;
using System.Collections.Generic;

namespace SpeechToText
{
    [Activity(Label = "PoC_Continous", MainLauncher = true, Icon = "@drawable/icon")] //Label: Name of Icon
    public class MainActivity : Activity, IRecognitionListener
    {
        public const string Tag = "VoiceRec"; //Used for debugging

        SpeechRecognizer Recognizer { get; set; } //Used to recognize voice input
        Intent SpeechIntent { get; set; } //Converts speech to text
        TextView Label { get; set; } //Displays all spoken text
        TextView Hit { get; set; } //Textbox when a term is found

        string[] jargon = { "TPS Report", "ACA", "Meeting Minutes", "XML", "Xamarin" }; //List of our terms
        List<string> knownstrings = new List<string>(); //Variable used to identify the string

        bool listening = false;  

        Button ThisButton { get; set; } //Mic off or on

        readonly string[] PermissionsRecord = //List of all permissions an app needs.
        {
            Manifest.Permission.RecordAudio
        };

        const int RequestRecordId = 0; //Verify which permission the grantor is pointing to

        protected override void OnCreate(Bundle bundle) 
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main); //Called when the application starts.  Creates UI

            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(this); //Instance of SpeechRecognizer
            Recognizer.SetRecognitionListener(this);

            SpeechIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech); //Intent: an intention to perform an action
            SpeechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, PackageName);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraPartialResults, true); //Identify text as it comes in

            ThisButton = FindViewById<Button>(Resource.Id.btnRecord); //The record button 
            ThisButton.Click += ButtonClick;

            Label = FindViewById<TextView>(Resource.Id.textYourText);
            Hit = FindViewById<TextView>(Resource.Id.resulthit);
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M) //If the version before API 23 (Marshmallow), no permission needs to be asked
            {
                const string permission = Manifest.Permission.RecordAudio; 
                if (CheckSelfPermission(permission) == (int)Permission.Granted) //Checking if the permission is granted
                {
                    if (listening)
                    {
                        Recognizer.StopListening();
                        ThisButton.Text = "Start Recording";
                        listening = false;
                    }
                    else
                    {
                        listening = true;
                        ThisButton.Text = "Stop Recording";
                        Recognizer.StartListening(SpeechIntent);
                    }
                }
                else
                {
                    RequestPermissions(PermissionsRecord, RequestRecordId); //Requests for permissions to record audio
                }
            }
            else
            {
                if (listening)  //Control logic to record audio for before API 23
                {
                    Recognizer.StopListening();
                    ThisButton.Text = "Start Recording";
                    listening = false;
                }
                else
                {
                    listening = true;
                    ThisButton.Text = "Stop Recording";
                    Recognizer.StartListening(SpeechIntent);
                }
            }
        }

        //When  the permission is granted
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestRecordId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            //Permission granted
                            if (listening)
                            {
                                Recognizer.StopListening();
                                ThisButton.Text = "Start Recording";
                                listening = false;
                            }
                            else
                            {
                                listening = true;
                                ThisButton.Text = "Stop Recording";
                                Recognizer.StartListening(SpeechIntent);
                            }
                        }
                    }
                    break;
            }
        }
        public void OnResults(Bundle results) 
        {
            var matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches != null && matches.Count > 0) //Checks when the conversation stops.
            {
                Label.Text = matches[0]; 
                foreach (string resulthitvalue in jargon) //Label.Text is set to result.  It checks for every term in array
                {
                    if (matches[0].ToLower().Contains(resulthitvalue.ToLower()))
                    {
                        if (!knownstrings.Contains(resulthitvalue))
                        {
                            Hit.Text = Hit.Text + "\r\n" + resulthitvalue; //Checking if the term in a sentence is found, but wasn't identified.
                        }
                    }

                }
                knownstrings.Clear(); //Clear list as a new brand new sentence to find the terms 
            }
            Recognizer.StartListening(SpeechIntent); //At the end of the sentence, the microphone listens again
        }
        public void OnReadyForSpeech(Bundle @params)
        {
            Log.Debug(Tag, "OnReadyForSpeech");
        }
        public void OnBeginningOfSpeech()
        {
            Log.Debug(Tag, "OnBeginningOfSpeech");
        }
        public void OnEndOfSpeech()
        {
            Log.Debug(Tag, "OnEndOfSpeech");
        }
        public void OnError([GeneratedEnum] SpeechRecognizerError error) //If there is an error, such as no speech is collected, the app stops listening.
        {
            Log.Debug("OnError", error.ToString());
            listening = false;
            ThisButton.Text = "Start Recording";
        }
        public void OnBufferReceived(byte[] buffer) { }
        public void OnEvent(int eventType, Bundle @params) { }
        public void OnPartialResults(Bundle partialResults) //Identifies text as it comes in
        {
            Log.Debug("found", "okay");
            IList<string> thisString = partialResults.GetStringArrayList(SpeechRecognizer.ResultsRecognition);//Gets partial result
            if (thisString != null)
            {
                if (thisString.Count > 0)  //This if collects the speech and concatenantes each word to be displayed as 
                {
                    Label.Text = thisString[0];
                    foreach (string resulthitvalue in jargon)
                    {
                        if (thisString[0].ToLower().Contains(resulthitvalue.ToLower()))
                        {
                            if (!knownstrings.Contains(resulthitvalue))
                            {
                                knownstrings.Add(resulthitvalue);
                                Hit.Text = Hit.Text + "\r\n" + resulthitvalue;
                            }
                        }
                    }
                }
            }
        }
        public void OnRmsChanged(float rmsdB) { }
    }
}