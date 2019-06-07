using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using _4H_Application.iOS.Models;
using _4H_Application.iOS.Views.Components;
using _4H_Application.Models.Settings;
using Foundation;
using UIKit;

[assembly: Dependency(typeof(Auth0Login))]
namespace _4H_Application.iOS.Models
{
    class Auth0Login : IAuth0Login
    {
        public async void AppLogin()
        {
            var loginResult = await AppDelegate.instance.client.LoginAsync();

            if(loginResult.IsError) {
                ToastMessage msg = new ToastMessage();
                msg.LongAlert("Error logging in: " + loginResult.Error);
            } else {
                AppSettings.Auth0Details.init();
                AppSettings.Auth0Details.identity = loginResult.User.FindFirst("sub")?.Value;
                AppSettings.Auth0Details.logged_in = true;
            }
        }

        public void AppLogout()
        {
            AppSettings.Auth0Details.init();
            UIApplication.SharedApplication.OpenUrl(new NSUrl("https://4h-app-backup.auth0.com/v2/logout"));
        }
    }
}