//
// _4H_Application.Views.Pages.MainPageMaster.xaml.cs: User interface class
//   that represents the navigation menu sidebar
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages {

    /// <summary>
    /// Class that represents the navigation menu sidebar
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage {

        // Reference to the list of menu items
        public ListView ListView;

        /// <summary>
        /// Default constructor for the navigation menu sidebar
        /// </summary>
        public MainPageMaster() {

            // Set the hamburger icon for the Master control
            switch (Device.RuntimePlatform) {
                case Device.Android:
                    this.Icon = "ic_menu_white";
                    break;
                case Device.iOS:
                    this.Icon = "ic_menu_black";
                    break;
            }

            // Initialize the component and bind the menu items context
            this.InitializeComponent();
            this.BindingContext = new MainPageMasterViewModel();
            this.ListView = this.MenuItemsListView;

        }

        /// <summary>
        /// Class that wraps the event listener for the menu items
        /// </summary>
        class MainPageMasterViewModel : INotifyPropertyChanged {

            // Reference to the menu items to the bound context of the page
            public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }

            /// <summary>
            /// Default constructor for the list of menu items
            /// </summary>
            public MainPageMasterViewModel() {

                // Initialize all of the different sidebar menu items
                MenuItems = new ObservableCollection<MainPageMenuItem>(new[] {
                    new MainPageMenuItem() {
                        Id = 1,
                        MenuTitle = "Profile",
                        TargetType = MainPage.PageType.ProfilePage,
                    },
                    new MainPageMenuItem() {
                        Id = 2,
                        MenuTitle = "Horses",
                        TargetType = MainPage.PageType.HorseManagerPage,
                    },
                    new MainPageMenuItem() {
                        Id = 3,
                        MenuTitle = "Record Book",
                        TargetType = MainPage.PageType.RecordBookPage,
                    },
                    new MainPageMenuItem() {
                        Id = 4,
                        MenuTitle = "Project",
                        TargetType = MainPage.PageType.ProjectPage,
                    },
                    new MainPageMenuItem() {
                        Id = 5,
                        MenuTitle = "Pictures",
                        TargetType = MainPage.PageType.PicturePage,
                    },
                    new MainPageMenuItem() {
                        Id = 6,
                        MenuTitle = "Export Record Books",
                        TargetType = MainPage.PageType.ExportPage,
                    },
                    /*
                    new MainPageMenuItem() {
                        Id = 7,
                        MenuTitle = "Record Backup",
                        TargetType = MainPage.PageType.BackupPage,
                    },
                    */
                });

            }

            /// <summary>
            /// Event handler for when the items are selected
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "") {

                // If the event was erraneously fired, ignore the event
                if (PropertyChanged == null) {
                    return;
                }

                // Invoke the event if it was not erraneously fired
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));

            }

        }

    }

}
