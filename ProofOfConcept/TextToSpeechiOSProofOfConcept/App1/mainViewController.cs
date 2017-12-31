using System;
using UIKit;
using Speech;
using Foundation;
using AVFoundation;
using CoreGraphics;
using System.Collections.Generic;

namespace App1
{
    public partial class mainViewController : UIViewController
    {

        //Manages whether the button should say "Stop Recording
        public bool isRecording = false;
        UITextView speechRecognitionTextView;
        UITextView hitRecognitionTextView;

        //Major Components For the speech Recognition on IOS devices
        AVAudioEngine AudioEngine = new AVAudioEngine();
        SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
        SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
        SFSpeechRecognitionTask RecognitionTask;

        //A List of our words that we will be looking for in the POC
        string[] hitWords = { "XML", "TPS Reports", "Meeting Minutes", "ACA", "Xamarin"};
        //A List of words already found in the text (So we dont print out the word twice)
        List<string> addedWords = new List<string>();

        public mainViewController() : base("mainViewController", null)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            

            nfloat h = 31.0f;
            nfloat w = View.Bounds.Width;

            //Button to press in order to start recording sound.
            var btn = UIButton.FromType(UIButtonType.System);
            //Set this button to a certain location at the top of the screen
            btn.Frame = new CGRect(10, 82, 200, 44);
            btn.SetTitle("Start Recording", UIControlState.Normal);

            //When we click on the button different functionality happens based on the recording status.
            btn.TouchUpInside += (sender, e) => {
                if (this.isRecording)
                {
                    CancelRecording();
                    this.isRecording = false;
                    btn.SetTitle("Start Recording", UIControlState.Normal);
                }
                else
                {
                    StartRecording();
                    this.isRecording = true;
                    btn.SetTitle("Stop Recording", UIControlState.Normal);
                }
            };

            View.AddSubview(btn);

            //This text View will serve as our "over all" voice recognition.
            speechRecognitionTextView = new UITextView
            {
                Text = "",
                Frame = new CGRect(10, 130, w - 40, h),
                Editable = false
            };
            View.AddSubview(speechRecognitionTextView);

            //This text View will serve as our "Recognized Terms" voice recongition
            hitRecognitionTextView = new UITextView
            {
                Text = "",
                Frame = new CGRect(10, 180, w - 40, h),
                Editable = false
            };

            View.AddSubview(hitRecognitionTextView);



            // Perform any additional setup after loading the view, typically from a nib.
            // Request user authorization
            SFSpeechRecognizer.RequestAuthorization((SFSpeechRecognizerAuthorizationStatus status) => {
                // Take action based on status
                switch (status)
                {
                    case SFSpeechRecognizerAuthorizationStatus.Authorized:
                    // User has approved speech recognition
                    
                    break;
                    case SFSpeechRecognizerAuthorizationStatus.Denied:
                    // User has declined speech recognition
                    
                    break;
                    case SFSpeechRecognizerAuthorizationStatus.NotDetermined:
                    // Waiting on approval
                    
                    break;
                    case SFSpeechRecognizerAuthorizationStatus.Restricted:
                    // The device is not permitted
                    
                    break;
                }
            });
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
                    //Put all the words that are recongized into the text box.
                    speechRecognitionTextView.Text = result.BestTranscription.FormattedString;
                    foreach(string word in hitWords)
                    {
                        if (speechRecognitionTextView.Text.ToLower().Contains(word.ToLower()))
                        {
                            //This word was recognized in the text box and was not already identified, we should at it to our "hits" box and our identified list.
                            if (!addedWords.Contains(word.ToLower()))
                            {
                                addedWords.Add(word.ToLower());
                                hitRecognitionTextView.Text = hitRecognitionTextView.Text + word.ToLower() + "\r\n"; 
                            }
                        }
                    }
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