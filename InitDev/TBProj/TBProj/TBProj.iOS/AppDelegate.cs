using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using AVFoundation;
using Speech;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace TBProj.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public Dictionary<string, string> cachelist;
        public List<string> searchlog;
        List<string> knownstrings = new List<string>(); //Variable used to keep list of known strings
        Task updater;

        const int tmp = 10; //used to verify request code
        string[] jargon = { "TPS Report", "Test", "Meeting Minutes", "XML", "Xamarin", "Ben", "Eli", "Chris", "Joe" }; //List of our terms
        public const string Tag = "VoiceRec"; //Used for debugging        
        public bool listening = false;
        public string ErrorMessage = "", ReturnHits = "", ReturnDescription = "", LastMessageRecorded = "";
        const int RequestRecordId = 0; //Verify which permission the grantor is pointing to

        AVAudioEngine AudioEngine = new AVAudioEngine();
        SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
        SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        SFSpeechRecognitionTask RecognitionTask;

        public static AppDelegate Self { get; private set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new TBProj.App());

            AppDelegate.Self = this;

            return base.FinishedLaunching(app, options);
        }

        public async void createcache()
        {
            cachelist = await startcache();

            if (updater != null)
            {
                return;
            }

            updater = Task.Run((Action)listenforchanges);
            await updater;
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

        public void sleeplistener()
        {
            StopRecording();
            listening = false;
            ErrorMessage = "ERROR!!!";
        }

        public async void ListenToggle()
        {

            SFSpeechRecognizer.RequestAuthorization((SFSpeechRecognizerAuthorizationStatus status) =>
            {
                // Take action based on status
                switch (status)
                {
                    case SFSpeechRecognizerAuthorizationStatus.Authorized:
                        // User has approved speech recognition

                        StartRecording();
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Denied:
                        // User has declined speech recognition

                        StopRecording();
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.NotDetermined:
                        // Waiting on approval

                        StopRecording();
                        break;
                    case SFSpeechRecognizerAuthorizationStatus.Restricted:
                        // The device is not permitted

                        StopRecording();
                        break;
                }
            });

        }

        public void clearHits()
        {
            ReturnHits = "";
        }

        public void StartRecording()
        {

            // Setup audio session
            var node = AudioEngine.InputNode;
            var recordingFormat = node.GetBusOutputFormat(0);
            node.InstallTapOnBus(0, 1024, recordingFormat, (AVAudioPcmBuffer buffer, AVAudioTime when) => {
                // Append buffer to recognition request
                LiveSpeechRequest.Append(buffer);
            });

            // Start recording
            AudioEngine.Prepare();
            NSError error;
            AudioEngine.StartAndReturnError(out error);

            // Did recording start?
            if (error != null)
            {
                // Handle error and return
                return;
            }

            // Start recognition
            RecognitionTask = SpeechRecognizer.GetRecognitionTask(LiveSpeechRequest, (SFSpeechRecognitionResult result, NSError err) => {
                // Was there an error?
                if (err != null)
                {
                    // Handle error
                }
                else
                {

                }
            });
        }


        public void StopRecording()
        {
            //When we stop the audio, we have to create a whole new instance of the recognizer for it to play nice.
            AudioEngine.Stop();
            LiveSpeechRequest.EndAudio();
            AudioEngine = new AVAudioEngine();
            SpeechRecognizer = new SFSpeechRecognizer();
            LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        }

        public void CancelRecording()
        {
            //When we stop the audio, we have to create a whole new instance of the recognizer for it to play nice.
            AudioEngine.Stop();
            RecognitionTask.Cancel();
            AudioEngine.InputNode.RemoveTapOnBus(0);
            AudioEngine = new AVAudioEngine();
            SpeechRecognizer = new SFSpeechRecognizer();
            LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        }

    }
}