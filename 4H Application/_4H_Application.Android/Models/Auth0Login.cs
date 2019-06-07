using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4H_Application.Droid.Models;
using _4H_Application.Models.Settings;
using Xamarin.Forms;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Dependency(typeof(Auth0Login))]
namespace _4H_Application.Droid.Models
{
    public class Auth0Login : IAuth0Login
    {
        public async void AppLogin()
        {
            MainActivity.instance.authState = await MainActivity.instance.client.PrepareLoginAsync();
            var uri = Android.Net.Uri.Parse(MainActivity.instance.authState.StartUrl);
            var intent = new Intent(Intent.ActionView, uri);
            intent.AddFlags(ActivityFlags.NoHistory);
            MainActivity.instance.StartActivity(intent);
        }

        public void AppLogout()
        {
            AppSettings.Auth0Details.init();
            var uri = Android.Net.Uri.Parse("https://4h-app-backup.auth0.com/v2/logout");
            var intent = new Intent(Intent.ActionView, uri);
            intent.AddFlags(ActivityFlags.NoHistory);
            MainActivity.instance.StartActivity(intent);
        }
    }
}