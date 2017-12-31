using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Speech;
using System.Collections.Generic;
using Android;
using Android.Util;
using Android.Support.V4.App;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using FFImageLoading.Forms.Droid;

namespace TBProj.Droid
{
    [Activity(Label = "Technobabel App", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IRecognitionListener, ActivityCompat.IOnRequestPermissionsResultCallback
    {

        const int tmp = 10; //used to verify request code
        string[] jargon = { "TPS Report", "Test", "Meeting Minutes", "XML", "Xamarin", "Ben", "Eli", "Chris", "Joe" }; //List of our terms
        List<string> knownstrings = new List<string>(); //Variable used to keep list of known strings

        public const string Tag = "VoiceRec"; //Used for debugging

        SpeechRecognizer Recognizer { get; set; } //Used to recognize voice input

        Intent SpeechIntent { get; set; } //Converts speech to text
        public bool listening = false;

        public string ErrorMessage = "";
        public string ReturnHits = "";
        Task updater;

        public string LastMessageRecorded = "";

        readonly string[] PermissionsRecord = //List of all permissions an app needs.
        {
            Manifest.Permission.RecordAudio
        };

        const int RequestRecordId = 0; //Verify which permission the grantor is pointing to

        public Dictionary<string, string> cachelist;
        public List<string> searchlog;


        protected override void OnCreate(Bundle bundle)
        {
            searchlog = new List<string>();
            cachelist = new Dictionary<string, string>();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CachedImageRenderer.Init();

            base.OnCreate(bundle);

            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(this); //Instance of SpeechRecognizer
            Recognizer.SetRecognitionListener(this);

            SpeechIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech); //Intent: an intention to perform an action
            SpeechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, PackageName);
            SpeechIntent.PutExtra(RecognizerIntent.ExtraPartialResults, true); //Identify text as it comes in

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new TBProj.App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
        }

        //caled when voice recorder is done (activity is exiting)
        /*
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
        }*/
        public async void createcache()
        {
            cachelist = await startcache();

            if (updater != null) return;

            updater = Task.Run((Action)listenforchanges);
            await updater;
        }


        public void sleeplistener()
        {
            Recognizer.StopListening();
            Recognizer.Destroy();
            listening = false;
            ErrorMessage = "ERROR!!!";
        }

        public async void listenforchanges()
        {
            while (true)
            {
                string request = "/api/GetTermsChangedByCompIDTime?compID=" + App.CurrentApp.Globals.compID.ToString() + "&time=" + DateTime.Now.ToString();
                var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                var updatelistresult = JsonConvert.DeserializeObject<List<UpdateTermItem>>(json);

                foreach (UpdateTermItem UTI in updatelistresult)
                {
                    if (UTI.Active)
                    {
                        string termstripped = UTI.TermName.ToUpper();
                        termstripped = termstripped.Replace('\n', ' ');
                        termstripped = termstripped.Trim();

                        if (cachelist.ContainsKey(termstripped))
                        {
                            cachelist.Remove(termstripped);
                        }

                        cachelist.Add(termstripped, UTI.TermDescription);
                    }
                    else
                    {
                        string termstripped = UTI.TermName.ToUpper();
                        termstripped = termstripped.Replace('\n', ' ');
                        termstripped = termstripped.Trim();

                        if (cachelist.ContainsKey(termstripped))
                        {
                            cachelist.Remove(termstripped);
                        }
                    }
                }

                await Task.Delay(10000);
            }
        }

        async Task<Dictionary<string, string>> startcache()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string request = "/api/GetTermListByCompanyID?comid=" + App.CurrentApp.Globals.compID.ToString();
            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
            var termExistResult = JsonConvert.DeserializeObject<List<TermListView>>(json);


            foreach (TermListView TM in termExistResult)
            {
                string termstripped = TM.TermName.ToUpper();
                termstripped = termstripped.Replace('\n', ' ');
                termstripped = termstripped.Trim();
                result.Add(termstripped, TM.TermDescription);
            }

            return result;
        }

        public void SendEmail(string email)
        {
            //Device.OpenUri(new Uri("mailto:" + email + "?subject=Password&body=Here is your password: "));

        }

        public void sleeplistener()
        {
            Recognizer.StopListening();
             Recognizer.Destroy();
             listening = false;
             ErrorMessage = "ERROR!!!";
        }

        public async void ListenToggle()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M) //If the version before API 23 (Marshmallow), no permission needs to be asked
            {
                const string permission = Manifest.Permission.RecordAudio;
                if (CheckSelfPermission(permission) == (int)Permission.Granted) //Checking if the permission is granted
                {
                    if (listening)
                    {
                        Recognizer.StopListening();
                        Recognizer.Destroy();
                        listening = false;
                    }
                    else
                    {
                        listening = true;
<<<<<<< HEAD
=======
                        //cachelist = await updatecache();

>>>>>>> 5f82d9e8cb30382f50da3f12b6185c7e98b8f5a9
                        Recognizer.Destroy();
                        Recognizer = SpeechRecognizer.CreateSpeechRecognizer(this); //Instance of SpeechRecognizer
                        Recognizer.SetRecognitionListener(this);
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
                    Recognizer.Destroy();
                    listening = false;
                }
                else
                {
                    listening = true;
<<<<<<< HEAD
=======
                    //cachelist = await updatecache();

>>>>>>> 5f82d9e8cb30382f50da3f12b6185c7e98b8f5a9
                    Recognizer.Destroy();
                    Recognizer = SpeechRecognizer.CreateSpeechRecognizer(this); //Instance of SpeechRecognizer
                    Recognizer.SetRecognitionListener(this);
                    Recognizer.StartListening(SpeechIntent);
                }
            }
        }


        public void clearHits()
        {
            ReturnHits = "";
        }

        //When  the permission is granted
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
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
                                Recognizer.Destroy();
                                //This is handled in the XAML
                                //ThisButton.Text = "Start Recording";
                                listening = false;
                            }
                            else
                            {
                                listening = true;
<<<<<<< HEAD
=======
                                //cachelist = await updatecache();
                                //This is handled in the XAML
                                //ThisButton.Text = "Stop Recording";

>>>>>>> 5f82d9e8cb30382f50da3f12b6185c7e98b8f5a9
                                Recognizer.Destroy();
                                Recognizer = SpeechRecognizer.CreateSpeechRecognizer(this); //Instance of SpeechRecognizer
                                Recognizer.SetRecognitionListener(this);
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
                string message = "";
                if (LastMessageRecorded != "")
                {
                    message = matches[0].Replace(LastMessageRecorded, "");
                    if (cachelist.ContainsKey(matches[0].ToUpper()))
                    {
                        knownstrings.Add(message.ToUpper());
                        string descript = "";
                        cachelist.TryGetValue(message.ToUpper(), out descript);
                        ReturnHits = message.ToUpper() + ": " + descript;
                    }
                }
                else
                {
                    message = matches[0];
                }

                if (message != "")
                {

                    Log.Debug("Term:", message);
                    if (cachelist.ContainsKey(message.ToUpper()))
                    {
                        knownstrings.Add(message.ToUpper());
                        string descript = "";
                        cachelist.TryGetValue(message.ToUpper(), out descript);
                        ReturnHits = message.ToUpper() + ": " + descript;
                    }
                    message = message.Replace(" ", "");

                    searchlog.Add(message.ToUpper());
                    if (searchlog.Count > 5)
                    {
                        searchlog.Remove(searchlog[0]);
                    }

                    if (cachelist.ContainsKey(message.ToUpper()))
                    {
                        knownstrings.Add(message.ToUpper());
                        string descript = "";
                        cachelist.TryGetValue(message.ToUpper(), out descript);
                        ReturnHits = message.ToUpper() + ": " + descript;
                    }

                    for (int i = searchlog.Count - 2; i >= 0; i--)
                    {
                        message = searchlog[i] + " " + message;
                        if (cachelist.ContainsKey(message.ToUpper()))
                        {
                            knownstrings.Add(message.ToUpper());
                            string descript = "";
                            cachelist.TryGetValue(message.ToUpper(), out descript);
                            ReturnHits = message.ToUpper() + ": " + descript;
                        }
                    }

                    /*
                   string request = "/api/TermExistByCompanyID?term=" + message + "&compid=" + App.CurrentApp.Globals.compID.ToString();
                   var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                   var termExistResult = JsonConvert.DeserializeObject<JsonSingleResult>(json).Result;
                   if (termExistResult == 1)
                   {
                       knownstrings.Add(message);
                       ReturnHits = message;
                   }*/
                }

                LastMessageRecorded = "";
                //ReturnVoiceRecording = matches[0];
                /*
                foreach (string resulthitvalue in jargon) //Label.Text is set to result.  It checks for every term in array
                {
                    if (matches[0].ToLower().Contains(resulthitvalue.ToLower()))
                    {
                        if (!knownstrings.Contains(resulthitvalue))
                        {
                            ReturnHits = resulthitvalue; //Checking if the term in a sentence is found, but wasn't identified.
                        }
                    }
                }*/
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
<<<<<<< HEAD
=======
            //listening = false;
            //ErrorMessage = "ERROR!!!";
>>>>>>> 5f82d9e8cb30382f50da3f12b6185c7e98b8f5a9
            Recognizer.Destroy();
            Recognizer = SpeechRecognizer.CreateSpeechRecognizer(this); //Instance of SpeechRecognizer
            Recognizer.SetRecognitionListener(this);
            Recognizer.StartListening(SpeechIntent);
        }

        public void OnBufferReceived(byte[] buffer) { }
        public void OnEvent(int eventType, Bundle @params) { }

        public void OnPartialResults(Bundle partialResults) //Identifies text as it comes in
        {

            IList<string> thisString = partialResults.GetStringArrayList(SpeechRecognizer.ResultsRecognition);//Gets partial result
            if (thisString != null)
            {
                string message = "";

                if (thisString.Count > 0)  //This if collects the speech and concatenantes each word to be displayed as 
                {
                    if (LastMessageRecorded != "")
                    {
                        message = thisString[0].Replace(LastMessageRecorded, "");
                        if (cachelist.ContainsKey(thisString[0].ToUpper()))
                        {
                            knownstrings.Add(message.ToUpper());
                            string descript = "";
                            cachelist.TryGetValue(message.ToUpper(), out descript);
                            ReturnHits = message.ToUpper() + ": " + descript;
                        }

                        if (message != "")
                        {
                            Log.Debug("Term:", message);

                            if (cachelist.ContainsKey(message.ToUpper()))
                            {
                                knownstrings.Add(message.ToUpper());
                                string descript = "";
                                cachelist.TryGetValue(message.ToUpper(), out descript);
                                ReturnHits = message.ToUpper() + ": " + descript;
                            }
                            message = message.Replace(" ", "");

                            searchlog.Add(message.ToUpper());
                            if (searchlog.Count > 5)
                            {
                                searchlog.Remove(searchlog[0]);
                            }

                            if (cachelist.ContainsKey(message.ToUpper()))
                            {
                                knownstrings.Add(message.ToUpper());
                                string descript = "";
                                cachelist.TryGetValue(message.ToUpper(), out descript);
                                ReturnHits = message.ToUpper() + ": " + descript;
                            }

                            for (int i = searchlog.Count - 2; i >= 0; i--)
                            {
                                message = searchlog[i] + " " + message;
                                if (cachelist.ContainsKey(message.ToUpper()))
                                {
                                    knownstrings.Add(message.ToUpper());
                                    string descript = "";
                                    cachelist.TryGetValue(message.ToUpper(), out descript);
                                    ReturnHits = message.ToUpper() + ": " + descript;
                                }
                            }

                            //string request = "/api/TermExistByCompanyID?term=" + message + "&compid=" + App.CurrentApp.Globals.compID.ToString();
                            //var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                            //var termExistResult = JsonConvert.DeserializeObject<JsonSingleResult>(json).Result;
                            //if (termExistResult == 1)
                            //{
                            //    knownstrings.Add(message);
                            //    ReturnHits = message;
                            //}
                        }
                    }
                    if (LastMessageRecorded == "" && thisString[0] != "")
                    {
                        string[] beginningList = thisString[0].Split(' ');
                        string xmessage = "";
                        foreach (string x in beginningList)
                        {
                            searchlog.Add(x.ToUpper());
                            if (searchlog.Count > 5)
                            {
                                searchlog.Remove(searchlog[0]);
                            }

                            if (cachelist.ContainsKey(x.ToUpper()))
                            {
                                knownstrings.Add(x.ToUpper());
                                string descript = "";
                                cachelist.TryGetValue(x.ToUpper(), out descript);
                                ReturnHits = x.ToUpper() + ": " + descript;
                            }

                            xmessage = x;

                            for (int i = searchlog.Count - 2; i >= 0; i--)
                            {
                                xmessage = searchlog[i] + " " + xmessage;
                                if (cachelist.ContainsKey(xmessage.ToUpper()))
                                {
                                    knownstrings.Add(xmessage.ToUpper());
                                    string descript = "";
                                    cachelist.TryGetValue(xmessage.ToUpper(), out descript);
                                    ReturnHits = xmessage.ToUpper() + ": " + descript;
                                }
                            }
                            /*
                            string request = "/api/TermExistByCompanyID?term=" + x + "&compid=" + App.CurrentApp.Globals.compID.ToString();
                            var json = App.CurrentApp.OurClient.GetStringAsync(request).Result;
                            var termExistResult = JsonConvert.DeserializeObject<JsonSingleResult>(json).Result;
                            if (termExistResult == 1)
                            {
                                knownstrings.Add(x);
                                ReturnHits = x;
                            }
                            */
                        }
                    }
                    LastMessageRecorded = thisString[0];

                    //ReturnVoiceRecording = thisString[0];
                    /*
                    foreach (string resulthitvalue in jargon)
                    {
                        if (thisString[0].ToLower().Contains(resulthitvalue.ToLower()))
                        {
                            if (!knownstrings.Contains(resulthitvalue))
                            {
                                knownstrings.Add(resulthitvalue);
                                ReturnHits = resulthitvalue;
                            }
                        }
                    }
                    */
                }
            }
        }
        public void OnRmsChanged(float rmsdB) { }

    }
}