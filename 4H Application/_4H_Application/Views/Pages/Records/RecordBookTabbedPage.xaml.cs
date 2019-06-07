//
// _4H_Application.Views.Pages.Records.RecordBookTabbedPage.xaml.cs: Tabbed
//   page that allows the user to manipulate their record book.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Models.Horses;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Records {

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordBookTabbedPage : TabbedPage {

        /// <summary>
        /// Reference to the selected date.
        /// </summary>
        public static DateTime SelectedDate {
            get; set;
        }

        /// <summary>
        /// Reference to the active instance of the record book tabbed page.
        /// </summary>
        private static RecordBookTabbedPage Instance {
            get; set;
        } = null;

        /// <summary>
        /// Default constructor for the tabbed page.
        /// </summary>
        public RecordBookTabbedPage() {

            // Initialize the component and set the current date
            RecordBookTabbedPage.SelectedDate = DateTime.Today;

            // Set the bar color if this is an Android device
            if (Device.RuntimePlatform == Device.Android) {
                this.BarBackgroundColor = (Color)
                    Application.Current.Resources["ApplicationGreen"];
            }
            
            // Initialize the component
            InitializeComponent();
            this.OnAppearing();

            // Set this as the current instance
            RecordBookTabbedPage.Instance = this;

        }

        /// <summary>
        /// Method that is first called when the page appears.
        /// </summary>
        protected override void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            // Check if the active horse is null - if so, display an error
            // message and navigate to the horse management page
            Device.BeginInvokeOnMainThread(async () => {

                // Check if there is no active/created horse
                if (HorseManager.GetInstance().ActiveHorse != null) {
                    return;
                }

                // Wait one second to make sure the page loaded properly
                await Task.Delay(500);

                // Indicate that the user will need to create a horse
                await this.DisplayAlert("Alert",
                    "Please make a horse before you manage your record book.",
                    "Ok");

                // Navigate to the horse management page
                MainPage.NavigateTo(MainPage.PageType.HorseManagerPage);

            });

        }

        /// <summary>
        /// Method that will navigate to the specified page.
        /// </summary>
        /// <param name="page">The page to navigate to.</param>
        public static void NavigateTo(TabbedPageOption page) {

            Device.BeginInvokeOnMainThread(() => {

                // Handle each type of page
                switch (page) {

                    case TabbedPageOption.CalendarPage:
                        Instance.CurrentPage = Instance.CalendarPageReference;
                        break;

                    case TabbedPageOption.RecordSelectPage:
                        Instance.CurrentPage = Instance.RecordPageReference;
                        break;

                    default:
                        break;

                }

            });

        }

        /// <summary>
        /// Enum that holds all of the values for the page navigation.
        /// </summary>
        public enum TabbedPageOption {
            CalendarPage,
            RecordSelectPage,
        }


    }

}