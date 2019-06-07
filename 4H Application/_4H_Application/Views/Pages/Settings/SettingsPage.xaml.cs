//
// _4H_Application.Views.Pages.Settings.SettingsPage.xaml.cs: Page that allows
//   the user to configure their settings and personal information
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Models.Settings;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Settings {

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage {

        /// <summary>
        /// Constructor for the settings page.
        /// </summary>
        public SettingsPage() {

            // Initialize the component
            this.BindingContext = new SettingsBinding();
            InitializeComponent();

        }

        /// <summary>
        /// Method that is called when the page is navigated away from.
        /// </summary>
        protected override void OnDisappearing() {

            // All of the information on the page is pretty basic and cannot be
            // messed up horribly, so save the information on the page as soon
            // as it is navigated away from

            // User Information
            AppSettings.User.UserName = this.UserNameEntry.Text;
            AppSettings.User.DateOfBirth = this.UserDateOfBirthDatePicker.Date;
            AppSettings.User.Address = this.UserAddressEditor.Text;

            // Horse Club Information
            AppSettings.HorseClub.ClubName = this.HorseClubNameEntry.Text;
            AppSettings.HorseClub.County = this.HorseClubCountyEntry.Text;
            AppSettings.HorseClub.LeaderName = this.HorseClubLeaderNameEntry.Text;

            // Horse Program Information
            AppSettings.HorseProgram.HelperName = this.ProjectHelperNameEntry.Text;
            AppSettings.HorseProgram.Level = (AppSettings.HorseProgram.ProgramLevel)
                (this.AchievementProgramLevelPicker.SelectedIndex + 1);
            AppSettings.HorseProgram.Year = (AppSettings.HorseProgram.ProgramYear)
                (this.AchievementProgramYearPicker.SelectedIndex + 1);
            AppSettings.HorseProgram.StartDate = this.AchievementProgramStartDatePicker.Date;
            AppSettings.HorseProgram.CloseDate = this.AchievementProgramCloseDatePicker.Date;

            // Application Settings
            AppSettings.Application.WeightUnits = (AppSettings.Application.WeightUnit)
                this.ApplicationWeightUnitsPicker.SelectedIndex;

        }

        /// <summary>
        /// Internal class to use as a binding context for the page.
        /// </summary>
        public class SettingsBinding {

            // User Settings
            public string UserName => AppSettings.User.UserName;
            public DateTime UserDateOfBirth => AppSettings.User.DateOfBirth;
            public string UserAddress => AppSettings.User.Address;

            // Horse Club Settings
            public string HorseClubName => AppSettings.HorseClub.ClubName;
            public string HorseClubCounty => AppSettings.HorseClub.County;
            public string HorseClubLeaderName => AppSettings.HorseClub.LeaderName;

            // Horse Program Settings
            public string HorseProgramHelperName => AppSettings.HorseProgram.HelperName;
            public AppSettings.HorseProgram.ProgramLevel HorseProgramLevel => AppSettings.HorseProgram.Level;
            public AppSettings.HorseProgram.ProgramYear HorseProgramYear => AppSettings.HorseProgram.Year;
            public DateTime HorseProgramStartDate => AppSettings.HorseProgram.StartDate;
            public DateTime HorseProgramCloseDate => AppSettings.HorseProgram.CloseDate;

            // Application Settings
            public AppSettings.Application.WeightUnit ApplicationWeightUnits => AppSettings.Application.WeightUnits;

            /// <summary>
            /// Default constructor for the binding object.
            /// </summary>
            public SettingsBinding() {
            }

        }

    }

    /// <summary>
    /// Converter for the program level.
    /// </summary>
    public class HorseProgramLevelConverter :
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

            // Reduce the value by 1 (since First = 1)
            return ((int)value) - 1;

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
    /// Converter for the program level.
    /// </summary>
    public class HorseProgramYearConverter :
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

            // Reduce the value by 1 (since First = 1)
            return ((int)value) - 1;

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