using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Java.Interop;
using System.Threading.Tasks;
using System.Security.Claims;
using Auth0.OidcClient;
using IdentityModel.OidcClient;
using _4H_Application.Droid.Data;
using _4H_Application.Droid.Views.Components;
using _4H_Application.Droid.Models.Exports;
using _4H_Application.Models.Settings;
using _4H_Application.Views.Pages;
using Android.Runtime;
using Plugin.Permissions;

namespace _4H_Application.Droid {

    [Activity(
        Label = "4H Horse",
        Icon = "@drawable/appicon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTask,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(
        new[] { Intent.ActionView },
        Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
        DataScheme = "com.fourh.recordbook",
        DataHost = "4h-app-backup.auth0.com",
        DataPathPrefix = "/android/com.fourh.recordbook/callback")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {

        private App app;

        // Accessible instance of the main activity
        internal static MainActivity instance { get; private set; }

        // Auth0 instance
        internal Auth0Client client;
        internal AuthorizeState authState;

        /// <summary>
        /// Create method for the Android application.
        /// </summary>
        /// <param name="bundle">Bundle to bind to.</param>
        protected override void OnCreate(Bundle bundle) {
            
            // Bind any dependency services
            Xamarin.Forms.DependencyService.Register<FileHelper>();
            Xamarin.Forms.DependencyService.Register<ExportHelper>();
            Xamarin.Forms.DependencyService.Register<ToastMessage>();

            // Android methods
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            // Auth0 client
            client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "4h-app-backup.auth0.com",
                ClientId = "0C6c4T69h4jtELXVf64wanm6331YiM77",
                Scope = "openid",
                Activity = this
            });

            // Instance of main activity
            instance = this;

        base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.Forms.Forms.ViewInitialized += (object sender, Xamarin.Forms.ViewInitializedEventArgs e) => {
                if (!string.IsNullOrWhiteSpace(e.View.AutomationId)) {
                    e.NativeView.ContentDescription = e.View.AutomationId;
                }
            };
            app = new App();
            LoadApplication(app);

            // Set the CrossActivity value
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;

        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            //var loginInfo = await client.ProcessResponseAsync(intent.DataString, authState);
            var task = Task.Run(async () => await client.ProcessResponseAsync(intent.DataString, authState));
            task.Wait();
            var loginInfo = task.Result;
            if(loginInfo.IsError) {
                ToastMessage msg = new ToastMessage();
                msg.LongAlert("Error logging in: " + loginInfo.Error);
            }
            else {
                AppSettings.Auth0Details.init();
                AppSettings.Auth0Details.identity = loginInfo.User.FindFirst("sub")?.Value;
                AppSettings.Auth0Details.logged_in = true;
            }
        }

        /// <summary>
        /// Used by the UI Testing program to navigate to other pages
        /// </summary>
        /// <param name="dest"></param>
        [Export("ChangePage")]
        public void ChangePage(string dest)
        {
            MainPage.PageType gohere;

            switch(dest) {
                case "Profile":
                    gohere = MainPage.PageType.ProfilePage;
                    break;
                case "HorseManager":
                    gohere = MainPage.PageType.HorseManagerPage;
                    break;
                case "RecordBook":
                    gohere = MainPage.PageType.RecordBookPage;
                    break;
                case "Project":
                    gohere = MainPage.PageType.ProfilePage;
                    break;
                case "Pictures":
                    gohere = MainPage.PageType.PicturePage;
                    break;
                case "Export":
                    gohere = MainPage.PageType.ExportPage;
                    break;
                default:
                    throw new System.ArgumentException("Unrecognized page destination");
            }

            MainPage.NavigateTo(gohere);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults) {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

    }

}

