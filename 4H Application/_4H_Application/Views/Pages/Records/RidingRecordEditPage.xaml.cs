//
// _4H_Application.Views.Pages.Records.RidingRecordEditPage.xaml.cs: Page
//   that serves as a view for the user to edit a single riding record.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Records;
using _4H_Application.Views.Components;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Records {

    /// <summary>
    /// Page that serves to edit a single riding record.
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RidingRecordEditPage : ContentPage {

        /// <summary>
        /// Reference to the current entry on the page.
        /// </summary>
        private RidingRecordEntry Entry;

        /// <summary>
        /// Reference to whether the page has been modified.
        /// </summary>
        private bool _modified = false;

        /// <summary>
        /// Default constructor for the page
        /// </summary>
		public RidingRecordEditPage(RidingRecordEntry entry) {

            // Initialize the component
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = this.Entry = entry;
            InitializeComponent();

            // Set the "modified" property of the page to false
            this._modified = false;

        }

        /// <summary>
        /// Method that is called when the start time button is clicked.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private void StartTimeButton_Clicked(object sender,
            EventArgs e) {

            // Set the TimePicker value to the current time
            this.StartTimePicker.Time = DateTime.Now.TimeOfDay;

        }

        /// <summary>
        /// Method that is called when the end time button is clicked.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private void EndTimeButton_Clicked(object sender,
            EventArgs e) {

            // Set the TimePicker value to the current time
            this.EndTimePicker.Time = DateTime.Now.TimeOfDay;

        }

        /// <summary>
        /// Method that is called when the save button is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private async void SaveButton_Clicked(object sender,
            EventArgs e) {

            // Make sure the end time is greater than the start time
            if (this.StartTimePicker.Time > this.EndTimePicker.Time) {
                ToastController.ShortToast("Error: End time must come after start time.");
                return;
            }

            // Save the entry with the values that are entered
            this.Entry.Description = this.DescriptionEditor.Text;
            this.Entry.Start = this.StartTimePicker.Time;
            this.Entry.End = this.EndTimePicker.Time;
            await AppDatabase.GetInstance().Save
                <RidingRecordEntry>(this.Entry);

            // Return to the previous page
            await this.Navigation.PopAsync(true);

        }

        /// <summary>
        /// Method that is called when the cancel button is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private void CancelButton_Clicked(object sender, EventArgs e) {

            Device.BeginInvokeOnMainThread(async () => {

                // Check if the page has not been modified
                if (this._modified == false) {
                    await Navigation.PopAsync(true);
                    return;
                }

                // Display a confirmation dialog asking whether the user actually
                // wants to discard the modified information.
                bool result = await this.DisplayAlert(
                    "Confirmation",
                    "Are you sure you want to discard any changes made to this entry?",
                    "Discard",
                    "Cancel");

                // If the user selected cancel, exit from the lambda
                if (!result) return;

                // Navigate back without saving the entry
                await Navigation.PopAsync(true);

            });

        }

        /// <summary>
        /// Method that is called when the delete button is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private void DeleteButton_Clicked(object sender,
            System.EventArgs e) {

            Device.BeginInvokeOnMainThread(async () => {

                // Display a confirmation dialog asking whether the user actually
                // wants to delete the specified entry.
                bool result = await this.DisplayAlert(
                    "Confirmation",
                    "Are you sure you want to delete this entry?  This action cannot be undone and all associated data will be deleted.",
                    "Delete",
                    "Cancel");

                // If the user selected cancel, exit from the lambda
                if (!result) return;

                // Delete the entry, then navigate back a page
                await AppDatabase.GetInstance().Delete<RidingRecordEntry>(this.Entry);
                await Navigation.PopAsync(true);

            });

        }

        /// <summary>
        /// Override for when the back button is pressed on Android devices.
        /// </summary>
        /// <returns>True, to suppress the navigation.</returns>
        protected override bool OnBackButtonPressed() {
            return true;
        }

        /// <summary>
        /// Method that is called when a field is modified.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void FieldModified(object sender,
            EventArgs e) {

            // Set the "modified" property of the page
            this._modified = true;

        }

    }

}