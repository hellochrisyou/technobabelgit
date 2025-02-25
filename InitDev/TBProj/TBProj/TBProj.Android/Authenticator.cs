﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;
using Android.Webkit;

[assembly: Xamarin.Forms.Dependency(typeof(TBProj.Droid.Authenticator))]
namespace TBProj.Droid
{
    class Authenticator : IAuthenticator
    {
        public async Task<AuthenticationResult> Authenticate(string authority, string resource, string clientId, string returnUri)
        {
            var authContext = new AuthenticationContext(authority);
            if (authContext.TokenCache.ReadItems().Any())
                authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);

            var uri = new Uri(returnUri);
            var platformParams = new PlatformParameters((Activity)Forms.Context);
            var authResult = await authContext.AcquireTokenAsync(resource, clientId, uri, platformParams);
            return authResult;
        }
        public void Logout()
        {
            CookieManager.Instance.RemoveAllCookie();
            TokenCache.DefaultShared.Clear();
        }
    }
}