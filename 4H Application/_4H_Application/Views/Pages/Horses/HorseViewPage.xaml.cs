//
// _4H_Application.Views.Horses.HorseViewPage.cs: Page that serves as a view 
//   for the user to inspect the information about a single horse.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Models.Horses;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Horses {

    /// <summary>
    /// Page that serves as a view for the user to view a single horse.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HorseViewPage : ContentPage {

        /// <summary>
        /// Reference to the horse that is bound to the page.
        /// </summary>
        private Horse horse;

        /// <summary>
        /// Default constructor for the page.
        /// </summary>
        /// <param name="horse">The horse to view.</param>
        public HorseViewPage(Horse horse) {

            // Remove the navigation bar from this page
            NavigationPage.SetHasNavigationBar(this, false);

            // Set the horse on the page and initialize the component
            this.BindingContext = this.horse = horse;
            this.InitializeComponent();

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

                // Re-populate the horse information
                this.HorseNameLabel.Text = this.horse.Name;
                this.HorseAgeLabel.Text = string.Format("{0} {1}",
                    this.horse.Age, this.horse.Age == 1 ? "year" : "years");
                this.HorseSexLabel.Text = this.horse.Sex.ToString();
                this.HorseBreedLabel.Text = this.horse.Breed.Replace(";", ", ");

                // Re-populate the horse appearance
                this.HorseHeightLabel.Text = string.Format("{0} {1}",
                    this.horse.Height == 0 ? "-" : this.horse.Height.ToString(),
                    this.horse.Height == 0 ? "" : this.horse.GetHeightUnits());
                this.HorseWeightLabel.Text = string.Format("{0} {1}",
                    this.horse.Weight == 0 ? "-" : this.horse.Weight.ToString(),
                    this.horse.Weight == 0 ? "" : this.horse.GetWeightUnits());
                this.HorseColorLabel.Text = this.horse.Color;
                this.HorseMarkingsLabel.Text = this.horse.Markings;

                // Re-populate the horse pedigree
                this.PedigreeLabel.Text = this.horse.Pedigree;
                this.SireLabel.Text = this.horse.Sire;
                this.PaternalGrandSireLabel.Text = this.horse.PaternalGrandSire;
                this.PaternalGrandDamLabel.Text = this.horse.PaternalGrandDam;
                this.DamLabel.Text = this.horse.Dam;
                this.MaternalGrandSireLabel.Text = this.horse.MaternalGrandSire;
                this.MaternalGrandDamLabel.Text = this.horse.MaternalGrandDam;

                // Invalidate the screen layout to fit everything
                this.InvalidateMeasure();

            });

        }

        /// <summary>
        /// Reference to the navigation lock on the page.
        /// </summary>
        private bool locked = false;

        /// <summary>
        /// Method callback for when the edit button is clicked.
        /// </summary>
        /// <param name="sender">Object that sent the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void EditButton_Clicked(object sender, 
            System.EventArgs e) {

            // Only continue execution if the page is unlocked
            if (this.locked) return;
            this.locked = true;

            // Create the page to edit the horse and navigate to it
            Page page = new HorseEditPage(this.horse);
            await this.Navigation.PushAsync(page, true);

            // Unlock the current page after navigation has occurred
            this.locked = false;

        }

        /// <summary>
        /// Method callback for when the back button is clicked.
        /// </summary>
        /// <param name="sender">Object that sent the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private async void BackButton_Clicked(object sender,
            System.EventArgs e) {

            // Only continue execution if the page is unlocked
            if (this.locked) return;
            this.locked = true;

            // Pop the horse view page off the navigation stack
            await this.Navigation.PopAsync();

            // Unlock the current page after navigation has occurred
            this.locked = false;

        }

    }

    /// <summary>
    /// Converter for the Horse sex.
    /// </summary>
    public class HorseSexConverter :
        IValueConverter {

        /// <summary>
        /// Method to convert the specified value.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <param name="targetType">The target control.</param>
        /// <param name="parameter">The parameter for the convert.</param>
        /// <param name="culture">Any culture info for the conversion.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {

            // Check if the instance is null - if so, return
            if (value == null) {
                return "";
            }

            // Return a string based on the specified type
            switch ((HorseSex) value) {

                case HorseSex.Colt:
                    return "Colt";

                case HorseSex.Filly:
                    return "Filly";

                case HorseSex.Gelding:
                    return "Gelding";

                case HorseSex.Mare:
                    return "Mare";

                default:
                    return "Colt";

            }

        }

        /// <summary>
        /// Method to convert the value back.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <param name="targetType">The target control.</param>
        /// <param name="parameter">The parameter for the convert.</param>
        /// <param name="culture">Any culture info for the conversion.</param>
        /// <returns>The original value.</returns>
        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {

            // Throw an exception since no implementation is necessary
            throw new NotImplementedException();

        }

    }

    /// <summary>
    /// Converter for the Horse breed.
    /// </summary>
    public class HorseBreedConverter :
        IValueConverter {

        /// <summary>
        /// Method to convert the specified value.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <param name="targetType">The target control.</param>
        /// <param name="parameter">The parameter for the convert.</param>
        /// <param name="culture">Any culture info for the conversion.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {

            // Check if the instance is null - if so, return
            if (value == null) {
                return "";
            }

            // Replace the semicolon delimiters with commas
            return ((string) value).Replace(";", ", ");

        }

        /// <summary>
        /// Method to convert the value back.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <param name="targetType">The target control.</param>
        /// <param name="parameter">The parameter for the convert.</param>
        /// <param name="culture">Any culture info for the conversion.</param>
        /// <returns>The original value.</returns>
        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {

            // Throw an exception since no implementation is necessary
            throw new NotImplementedException();

        }

    }

    /// <summary>
    /// Converter for the Horse height.
    /// </summary>
    public class HorseHeightConverter :
        IValueConverter {

        /// <summary>
        /// Method to convert the specified value.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <param name="targetType">The target control.</param>
        /// <param name="parameter">The parameter for the convert.</param>
        /// <param name="culture">Any culture info for the conversion.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {

            // Check if the instance is null - if so, return
            if (value == null) {
                return "";
            }

            // Format the string with the height units
            Horse horse = (Horse) value;
            return string.Format("{0} {1}", horse.Height, 
                horse.GetHeightUnits());

        }

        /// <summary>
        /// Method to convert the value back.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <param name="targetType">The target control.</param>
        /// <param name="parameter">The parameter for the convert.</param>
        /// <param name="culture">Any culture info for the conversion.</param>
        /// <returns>The original value.</returns>
        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {

            // Throw an exception since no implementation is necessary
            throw new NotImplementedException();

        }

    }

    /// <summary>
    /// Converter for the Horse height.
    /// </summary>
    public class HorseWeightConverter :
        IValueConverter {

        /// <summary>
        /// Method to convert the specified value.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <param name="targetType">The target control.</param>
        /// <param name="parameter">The parameter for the convert.</param>
        /// <param name="culture">Any culture info for the conversion.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {

            // Check if the instance is null - if so, return
            if (value == null) {
                return "";
            }

            // Format the string with the weight units
            Horse horse = (Horse)value;
            return string.Format("{0} {1}", horse.Weight,
                horse.GetWeightUnits());

        }

        /// <summary>
        /// Method to convert the value back.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <param name="targetType">The target control.</param>
        /// <param name="parameter">The parameter for the convert.</param>
        /// <param name="culture">Any culture info for the conversion.</param>
        /// <returns>The original value.</returns>
        public object ConvertBack(object value,
            Type targetType,
            object parameter,
            CultureInfo culture) {

            // Throw an exception since no implementation is necessary
            throw new NotImplementedException();

        }

    }

}