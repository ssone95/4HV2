//
// _4H_Application.Views.Pages.Pictures.PictureEditPage.xaml.cs: Class that
//   represents a page that allows the user to manage a single picture.

using _4H_Application.Models.Pictures;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Pictures {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PictureEditPage : ContentPage {

        /// <summary>
        /// Reference to the picture on the page.
        /// </summary>
        private Picture _Picture;

        private bool _modified = false;

        /// <summary>
        /// Default constructor for the PictureEditPage.
        /// </summary>
		public PictureEditPage(Picture picture) {

            // Disable the navigation bar from this page
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);

            // Render the page
            this.BindingContext = this._Picture = picture;
            InitializeComponent();

		}

        /// <summary>
        /// Method that is called when the page first appears.
        /// </summary>
        protected override void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            // Set the "modified" property of the page to false
            this._modified = false;

        }

        /// <summary>
        /// Override method to suppress the Android back button.
        /// </summary>
        /// <returns>True, to suppress the button press.</returns>
        protected override bool OnBackButtonPressed() {

            // Delegate to the "BackButton_Clicked" method
            this.BackButton_Clicked(null, null);

            // Return true to prevent navigation errors
            return true;

        }

        /// <summary>
        /// Method that is called when the save button is clicked.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the click event.</param>
        private async void SaveButton_Clicked(object sender, 
            System.EventArgs e) {
            
            // Check if the image was unmodified
            if (this._modified == false) {
                await Navigation.PopAsync(true);
                return;
            }

            // Update the picture caption and save the picture
            this._Picture.Caption = this.CaptionEntry.Text;
            await PictureManager.Instance.Save(this._Picture);

            // Return to the previous page
            await Navigation.PopAsync(true);

        }

        /// <summary>
        /// Method that is called when the back button is clicked.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the click event.</param>
        private void BackButton_Clicked(object sender, 
            System.EventArgs e) {

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
                    "Are you sure you want to discard any changes made to this picture?",
                    "Discard",
                    "Cancel");

                // If the user selected cancel, exit from the lambda
                if (!result) return;

                // Navigate back without saving the horse
                await Navigation.PopAsync(true);

            });

        }

        /// <summary>
        /// Method that is called when the remove button is clicked.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the click event.</param>
        private void RemoveButton_Clicked(object sender, 
            System.EventArgs e) {

            Device.BeginInvokeOnMainThread(async () => {

                // Display a confirmation dialog asking whether the user actually
                // wants to discard the modified information.
                bool result = await this.DisplayAlert(
                    "Confirmation",
                    "Are you sure you want to remove this image from the application?  The image will still be available from the device gallery.",
                    "Remove",
                    "Cancel");

                // If the user selected cancel, exit from the lambda
                if (!result) return;

                // Navigate back after deleting the picture reference
                await PictureManager.Instance.Remove(this._Picture);
                await Navigation.PopAsync(true);

            });

        }

        /// <summary>
        /// Method that is called when the caption text is changed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the change event.</param>
        private void CaptionEntry_TextChanged(object sender, 
            TextChangedEventArgs e) {

            // Set the "modified" property of the page to true
            this._modified = true;

        }

    }

}