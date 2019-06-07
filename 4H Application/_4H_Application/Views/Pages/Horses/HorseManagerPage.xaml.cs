//
// _4H_Application.Views.Horses.HorseManagerPage.cs: Page that allows a user to
//   add or remove a horse from their available horses, or edit an existing
//   horse.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Horses;
using _4H_Application.Models.Horses.Exceptions;
using _4H_Application.Views.Components;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Horses {

    /// <summary>
    /// Page that displays options for a user to interact with a horse.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HorseManagerPage : ContentPage {

        /// <summary>
        /// Boolean value to determine whether or not the main executing thread
        /// should be locked from having another modal being opened
        /// </summary>
        private bool locked = false;

        /// <summary>
        /// Default constructor for the horse manager page.
        /// </summary>
        public HorseManagerPage() {

            // Initialize the page and attach the listview ItemsSource
            InitializeComponent();

        }

        /// <summary>
        /// Override of the method that is called when the page first appears.
        /// </summary>
        protected override void OnAppearing() {

            // Call the base method first to prevent any initialization issues.
            base.OnAppearing();

            // Set the source of the horses on the page
            this.RefreshListView();

        }

        /// <summary>
        /// Internal method for refreshing the list view by toggling the data
        /// source.
        /// </summary>
        private void RefreshListView() {

            // Run this on the UI thread to prevent screen tearing.
            Device.BeginInvokeOnMainThread(() => {

                // Set the source to null and then back to the actual horse list
                this.HorseManagerListView.ItemsSource = null;
                this.HorseManagerListView.ItemsSource =
                    HorseManager.GetInstance().Horses;

            });

        }

        /// <summary>
        /// Method to be called when a ListView item is selected.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="ev">Event arguments for the selected event.</param>
        protected async void OnItemSelected(object sender,
            ItemTappedEventArgs ev) {

            // Check if the page is already locked
            if (this.locked) return;

            // Lock the current page from navigation
            this.locked = true;

            // Check if the sending context is null
            if (ev.Item == null) {
                return;
            }

            // Get the selected horse from the event arguments
            Horse horse = ev.Item as Horse;
            this.HorseManagerListView.SelectedItem = null;

            // If the horse is the active horse, open the viewing page
            if (HorseManager.GetInstance().ActiveHorse == horse) {

                // Open the viewing page for the selected horse
                Page page = new HorseViewPage(horse);
                await this.Navigation.PushAsync(page, true);

            }

            // Otherwise, set this horse as the active horse
            else {
                ToastController.ShortToast(string.Format(
                    "{0} set as active horse.", horse.Name));
                HorseManager.GetInstance().ActiveHorse = horse;
                this.RefreshListView();
            }

            // Unlock the current page after navigation has occurred
            this.locked = false;

        }

        /// <summary>
        /// Method to be called when the "add horse" button is pressed.
        /// </summary>
        /// <param name="sender">The item that triggers the event.</param>
        /// <param name="e">The event args for the event.</param>
        private void AddHorseButton_Clicked(object sender, EventArgs e) {

            // If the current modal page is set to be locked, do not render a
            // horse modal page
            if (this.locked) {
                return;
            }

            // Lock the main thread from opening another page until the current
            // modal display has been closed
            this.locked = true;

            // Invoke a separate operation to create a horse.
            Device.BeginInvokeOnMainThread(async () => {

                try {

                    // Prompt the user for the type of horse that they want to
                    // create, from the types that are available.
                    const string StandardHorseString = "Standard";
                    const string MiniatureHorseString = "Miniature";
                    const string CancelString = "Cancel";
                    string result = await this.DisplayActionSheet(
                        "Select A Horse Type", 
                        CancelString,
                        null,
                        StandardHorseString,
                        MiniatureHorseString);

                    // Handle the result accordingly
                    HorseType type = HorseType.Standard;
                    switch (result) {

                        // Handle when a standard horse is selected
                        case StandardHorseString:
                            type = HorseType.Standard;
                            break;

                        // Handle when a miniature horse is selected
                        case MiniatureHorseString:
                            type = HorseType.Miniature;
                            break;

                        // Handle when the "cancel" button was pressed, or the
                        // user selects outside the bounds of the select.
                        case CancelString:
                        default:
                            return;

                    }

                    // Get a new horse from the horse manager
                    Horse horse = await HorseManager.GetInstance()
                        .Create(type);

                    // Push a modal to the top of the navigation stack to
                    // display the selected horse's information
                    Page horsePage = new HorseEditPage(horse);
                    await this.Navigation.PushAsync(horsePage);

                } catch (TooManyHorsesException) {

                    // Show a toast indicating that the horse could not be found
                    ToastController.ShortToast("Cannot create any more horses.");

                } finally {

                    // Unlock the page after the modal has been created
                    this.locked = false;

                }

            });

        }

    }

}
