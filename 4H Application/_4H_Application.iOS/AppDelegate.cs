using Foundation;
using UIKit;
using Auth0.OidcClient;
using _4H_Application.iOS.Data;
using _4H_Application.iOS.Views.Components;
using _4H_Application.iOS.Models.Exports;

namespace _4H_Application.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        internal static AppDelegate instance;
        internal Auth0Client client;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            // Bind any dependency services
            Xamarin.Forms.DependencyService.Register<FileHelper>();
            Xamarin.Forms.DependencyService.Register<ExportHelper>();
            Xamarin.Forms.DependencyService.Register<ToastMessage>();
            Xamarin.Forms.DependencyService.Register<ExpandableEditorRenderer>();

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            var baseResult = base.FinishedLaunching(app, options);

            instance = this;
            client = new Auth0Client(new Auth0ClientOptions
            {
                Domain = "4h-app-backup.auth0.com",
                ClientId = "0C6c4T69h4jtELXVf64wanm6331YiM77",
                Scope = "openid",
                Controller = UIApplication.SharedApplication.KeyWindow.RootViewController // The instance of your UIViewController from which you are calling this code
            });

            return baseResult;
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            ActivityMediator.Instance.Send(url.AbsoluteString);

            return true;
        }
    }
}
