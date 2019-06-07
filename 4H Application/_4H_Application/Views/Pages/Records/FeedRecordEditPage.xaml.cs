//
// _4H_Application.Views.Pages.Records.FeedRecordEditPage.xaml.cs: Page
//   that serves as a view for the user to edit a single feed record.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Records;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Records {

    /// <summary>
    /// Page that allows for editing of a single feed record.
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FeedRecordEditPage : ContentPage {

        /// <summary>
        /// Reference to the current entry on the page.
        /// </summary>
        private FeedRecordEntry Entry;

        /// <summary>
        /// Reference to whether the entry has been modified.
        /// </summary>
        private bool _modified = false;

        /// <summary>
        /// Default constructor for the page.
        /// </summary>
		public FeedRecordEditPage(FeedRecordEntry entry) {

            // Initialize the page
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = this.Entry = entry;
            InitializeComponent();

            // Set the modified property of the page to "false"
            this._modified = false;

        }

        /// <summary>
        /// Method that renders the display based on the type selected.
        /// </summary>
        private void UpdateDisplay() {

            Device.BeginInvokeOnMainThread(() => {

                // Switch based on the type of feed record
                switch ((FeedType) this.TypePicker.SelectedIndex) {

                    case FeedType.Grain:
                    case FeedType.Hay:
                    case FeedType.SaltAndMinerals:
                    case FeedType.Pasture:

                        // Hide/display the appropriate fields
                        this.AmountGrid.IsVisible = true;
                        this.AmountSeparator.IsVisible = true;
                        break;

                    case FeedType.Other:

                        // Hide/display the appropriate fields
                        this.AmountGrid.IsVisible = false;
                        this.AmountSeparator.IsVisible = false;
                        break;

                }

            });

        }

        /// <summary>
        /// Method that is called when the type picker value is modified.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private void TypePicker_SelectedIndexChanged(object sender, 
            EventArgs e) {

            // Update the display
            this._modified = true;
            this.UpdateDisplay();

        }

        /// <summary>
        /// Method that is called when the save button is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private async void SaveButton_Clicked(object sender,
            EventArgs e) {

            // Save the entry with the values that are entered
            this.Entry.Type = (FeedType) this.TypePicker.SelectedIndex;
            this.Entry.Description = this.DescriptionEditor.Text;
            this.Entry.Amount = this.AmountEntry.Value;
            this.Entry.Cost = this.CostEntry.Value;
            await AppDatabase.GetInstance().Save
                <FeedRecordEntry>(this.Entry);

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

                // Check if the page has not yet been modified
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
                await AppDatabase.GetInstance().Delete<FeedRecordEntry>(this.Entry);
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