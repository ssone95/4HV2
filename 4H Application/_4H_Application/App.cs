//
// _4H_Application.App.cs: Creates the application
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Exports;
using _4H_Application.Models.Horses;
using _4H_Application.Models.Settings;
using _4H_Application.Models.Pictures;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Plugin.Media;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace _4H_Application {

    /// <summary>
    /// Class that serves as the entry point for the entire application
    /// </summary>
    public partial class App : Application {

        /// <summary>
        /// Default constructor for the 4H Application
        /// </summary>
        /// The constructor creates the default resources directory for the
        /// rest of the UI components to refer to, and creates the main page
        /// which the application defaults to.
        public App() {

            // Define all of the resources for the application
            this.Resources = new ResourceDictionary {
                { "ApplicationGreen", Color.FromRgb(0x00, 0x84, 0x4C) },
            };

            // Create the main page and set it as the default view for the
            // application
            this.MainPage = new Views.Pages.MainPage();

        }

        /// <summary>
        /// Method called when the application first opens.
        /// </summary>
        protected override async void OnStart() {

            // Start the media manager
            await CrossMedia.Current.Initialize();

            // Handle when your app starts
            HorseManager.GetInstance();
            await PictureManager.Instance.LoadPictures();

        }

        /// <summary>
        /// Method called when the application is closed via the "back" button.
        /// </summary>
        protected override void OnSleep() {

            // Handle when your app sleeps
            Debug.WriteLine("Hello, world!");

        }

        /// <summary>
        /// Method called when the application is re-opened.
        /// </summary>
        protected override void OnResume() {

            // Handle when your app resumes
            Debug.WriteLine("Hello, world!");

        }

    }

}
