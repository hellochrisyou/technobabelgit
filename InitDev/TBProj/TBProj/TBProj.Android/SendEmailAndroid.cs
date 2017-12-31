using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(TBProj.Droid.SendEmailAndroid))]
namespace TBProj.Droid
{
    class SendEmailAndroid: SendEmailInterface
    {
        public void SendEmail(string sendEmailTo)
        {
            var email = new Intent(Intent.ActionSend);

            email.PutExtra(Intent.ExtraEmail, new string[] { "Technobabelteam@gmail.com", "Technobabelteam@gmail.com" });
            email.PutExtra(Intent.ExtraSubject, "Forgotten Password");
            email.PutExtra(Intent.ExtraText, "Send password to: " + sendEmailTo);
            email.SetType("message/rfc822");

            ((MainActivity)Xamarin.Forms.Forms.Context).StartActivity(email);
            //((MainActivity)Xamarin.Forms.Forms.Context).SendEmail(email);
        }
    }
}