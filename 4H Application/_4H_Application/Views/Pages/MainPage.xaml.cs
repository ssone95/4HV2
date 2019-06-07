//
// _4H_Application.Views.MainPage.xaml.cs: Page that serves as the container
//   for all of the other pages in the application.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Models.Horses;
using _4H_Application.Views.Pages.Exports;
using _4H_Application.Views.Pages.Horses;
using _4H_Application.Views.Pages.Pictures;
using _4H_Application.Views.Pages.Projects;
using _4H_Application.Views.Pages.Records;
using _4H_Application.Views.Pages.Backup;
using _4H_Application.Views.Pages.Settings;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages {

    /// <summary>
    /// Class that serves as a user interface controller to navigate between
    /// all of the other available pages of the application.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage {

        /// <summary>
        /// Static instance of the page.
        /// </summary>
        private static MainPage _Instance;

        /// <summary>
        /// Dictionary of the application's pages
        /// </summary>
        private static Dictionary<PageType, NavigationPage> PageDictionary {
            get; set;
        } = null;

        /// <summary>
        /// Default constructor for the application main page.
        /// </summary>
        public MainPage() {

            // Create a static reference of this page to user for navigation
            MainPage._Instance = this;

            // Create the dictionary of detail pages in the application
            MainPage.PageDictionary = new Dictionary<PageType, NavigationPage>() {
                { PageType.ProfilePage, new NavigationPage(new SettingsPage()) },
                { PageType.HorseManagerPage, new NavigationPage(new HorseManagerPage()) },
                { PageType.RecordBookPage, new NavigationPage(new RecordBookTabbedPage()) },
                { PageType.ProjectPage, new NavigationPage(new ProjectSelectPage()) },
                { PageType.PicturePage, new NavigationPage(new PicturePage()) },
                { PageType.ExportPage, new NavigationPage(new ExportPage()) },
                { PageType.BackupPage, new NavigationPage(new BackupPage()) },
            };

            // Initialize the component and add the item selected callback
            // Initialize the component and add the item selected callback
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            // Set the home page based on the number of horses
            if (HorseManager.GetInstance().Horses.Count == 0) {
                this.Detail = MainPage.PageDictionary[PageType.HorseManagerPage];
            } else {
                this.Detail = MainPage.PageDictionary[PageType.RecordBookPage];
            }

            // Set the toolbar color on the current detail page if the device
            // is an Android device
            if (Device.RuntimePlatform == Device.Android) {
                ((NavigationPage) this.Detail).BarBackgroundColor = (Color)
                    Application.Current.Resources["ApplicationGreen"];
            }
            
        }

        /// <summary>
        /// Event callback for when the user selects an item from the sidebar
        /// navigation panel.
        /// </summary>
        /// <param name="sender">Object that created the event</param>
        /// <param name="e">Event that is created by the user</param>
        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e) {

            // Get the item specified by the event
            MainPageMenuItem item = e.SelectedItem as MainPageMenuItem;
            if (item == null) {
                return;
            }

            // Hide the sidebar
            this.MasterPage.ListView.SelectedItem = null;
            this.IsPresented = false;

            // Navigate to the specified page
            MainPage.NavigateTo(item.TargetType);

        }

        /// <summary>
        /// Method that will navigate to the specified page
        /// </summary>
        /// <param name="type"></param>
        public static void NavigateTo(PageType type) {

            Device.BeginInvokeOnMainThread(() => {

                // Get the page and make sure that the application is not already
                // actively on this page
                NavigationPage page = MainPage.PageDictionary[type];
                if (page == MainPage._Instance.Detail) {
                    return;
                }

                // Set the page that was selected
                MainPage._Instance.Detail = page;
                if (Device.RuntimePlatform == Device.Android) {
                    ((NavigationPage) MainPage._Instance.Detail).BarBackgroundColor =
                        (Color)Application.Current.Resources["ApplicationGreen"];
                }

            });

        }

        /// <summary>
        /// Method to be called on iOS to actually get the backup page to update.
        /// Attempting to udpate using OnAppearing only works when returning from a login, and it doesn't get the results quick enough on iOS
        /// Attempting to update using OnResume only works for logout, and even if you attempt do so it'll be messy
        /// </summary>
        public static void ResetBackupPage()
        {
            // only run this method for iOS - Android works like a dream without this method
            if(Device.RuntimePlatform == Device.iOS) {
                NavigateTo(PageType.HorseManagerPage);
                MainPage.PageDictionary[PageType.BackupPage] = new NavigationPage(new BackupPage());
                NavigateTo(PageType.BackupPage);
            }
        }

        /// <summary>
        /// Enum that holds all of the potential detail pages.
        /// </summary>
        public enum PageType {
            ProfilePage,
            HorseManagerPage,
            RecordBookPage,
            ProjectPage,
            PicturePage,
            ExportPage,
            BackupPage,
        };

    }

}
