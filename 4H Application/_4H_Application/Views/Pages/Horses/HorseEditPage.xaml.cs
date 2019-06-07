//
// _4H_Application.Views.Pages.Horses.HorseEditPage.xaml.cs: Page that serves 
//   as a view for the user to edit the information about a single horse.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Horses;
using _4H_Application.Views.Components;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Horses {

    /// <summary>
    /// Page that serves as a view for the user to edit a single horse.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HorseEditPage : ContentPage {

        /// <summary>
        /// Reference to the horse that is bound to the page.
        /// </summary>
        private Horse Horse;

        /// <summary>
        /// Reference to whether the page has been modified.
        /// </summary>
        private bool _modified = false;

        /// <summary>
        /// Default constructor for the page.
        /// </summary>
        /// <param name="horse">The horse to view.</param>
        public HorseEditPage(Horse horse) {

            // Set the appearance of the toolbar
            NavigationPage.SetHasNavigationBar(this, false);

            // Set the horse on the page and initialize the component
            this.BindingContext = this.Horse = horse;
            InitializeComponent();

        }

        /// <summary>
        /// Method that is called when the page first appears.
        /// </summary>
        protected override void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            // Invoke the form update on the UI thread to reduce the latency of
            // loading all of the data onto the page
            Device.BeginInvokeOnMainThread(() => {

                // Set the breed of the horse
                this.HorseBreedPicker.Breed = this.Horse.Breed;

                // Set the height of the horse
                this.HorseHeightLabel.Text = string.Format("Height\n({0})",
                    this.Horse.GetHeightUnits());

                // Set the weight of the horse
                this.HorseWeightLabel.Text = string.Format("Weight\n({0})",
                    this.Horse.GetWeightUnits());

                // Set the "modified" property to false
                this._modified = false;

            });

        }

        /// <summary>
        /// Method that is called when the save button is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private void SaveButton_Clicked(object sender, System.EventArgs e) {

            // Go through and validate all of the information that has been
            // entered before the saving process starts
            Device.BeginInvokeOnMainThread(async () => {

                // Check the horse's name
                if (this.HorseNameEntry.Text.Trim().Length < 2) {
                    ToastController.ShortToast("Horse name must be at least 2 characters.");
                    return;
                }

                // Check the horse's age
                if (!int.TryParse(this.HorseAgeEntry.Text, out int t1)) {
                    ToastController.ShortToast("Horse age must be a whole number.");
                    return;
                }

                // Check the horse's height
                if (!float.TryParse(this.HorseHeightEntry.Text, out float t2)) {
                    ToastController.ShortToast("Horse height is not in a valid format.");
                    return;
                }

                // Check the horse's weight
                if (!float.TryParse(this.HorseWeightEntry.Text, out float t3)) {
                    ToastController.ShortToast("Horse weight is not in a valid format.");
                    return;
                }

                // If all of the data was validated, save the horse
                await this.SaveHorse();

                // Navigate to the previous page after the horse has been saved
                await this.Navigation.PopAsync(true);

            });

        }

        /// <summary>
        /// Method to save the current horse.
        /// </summary>
        private async Task SaveHorse() {

            // Save the horse information fields
            this.Horse.Name = this.HorseNameEntry.Text;
            this.Horse.Age = int.Parse(this.HorseAgeEntry.Text);
            this.Horse.Sex = (HorseSex) (this.HorseSexPicker.SelectedIndex);
            this.Horse.Breed = this.HorseBreedPicker.Breed;

            // Save the horse appearance fields
            this.Horse.Height = float.Parse(this.HorseHeightEntry.Text);
            this.Horse.Weight = float.Parse(this.HorseWeightEntry.Text);
            this.Horse.Color = this.HorseColorEditor.Text;
            this.Horse.Markings = this.HorseMarkingsEditor.Text;

            // Save the horse pedigree fields
            this.Horse.Pedigree = this.PedigreeEntry.Text;
            this.Horse.Sire = this.SireEntry.Text;
            this.Horse.PaternalGrandSire = this.PaternalGrandSireEntry.Text;
            this.Horse.PaternalGrandDam = this.PaternalGrandDamEntry.Text;
            this.Horse.Dam = this.DamEntry.Text;
            this.Horse.MaternalGrandSire = this.MaternalGrandSireEntry.Text;
            this.Horse.MaternalGrandDam = this.MaternalGrandDamEntry.Text;

            // Save the horse to the database
            await AppDatabase.GetInstance().Save<Horse>(this.Horse);

        }

        /// <summary>
        /// Method that is called when the cancel button is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private void CancelButton_Clicked(object sender, System.EventArgs e) {

            // Check if the selected horse has just been created
            if (this.Horse.Name == string.Empty) {
                this.DeleteButton_Clicked(sender, e);
                return;
            }

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
                    "Are you sure you want to discard any changes made to this horse?",
                    "Discard",
                    "Cancel");

                // If the user selected cancel, exit from the lambda
                if (!result) return;

                // Navigate back without saving the horse
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
                // wants to delete the specified horse.
                bool result = await this.DisplayAlert(
                    "Confirmation",
                    "Are you sure you want to delete this horse?  This action cannot be undone and all associated data will be deleted.",
                    "Delete",
                    "Cancel");

                // If the user selected cancel, exit from the lambda
                if (!result) return;

                // Delete the horse, then navigate up the stack until the 
                // HorseManagerPage is reached.
                await HorseManager.GetInstance().Delete(this.Horse);
                await Navigation.PopToRootAsync(true);

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