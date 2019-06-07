//
// _4H_Application.Views.Pages.Project.ProjectExperiencesPage.xaml.cs: Page
//   that serves as a view for the project experiences based on their project
//   level.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause (jkjk8080@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Horses;
using _4H_Application.Models.Projects;
using _4H_Application.Models.Settings;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Projects {

    /// <summary>
    /// Class that shows all of the project experiences.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectExperiencesPage : ContentPage {

        /// <summary>
        /// Reference to the current horse.
        /// </summary>
        private Horse _horse = null;

        /// <summary>
        /// Reference to the list of experiences.
        /// </summary>
        private ProjectExperiences _experiences = null;

        /// <summary>
        /// Reference to whether the page has been changed.
        /// </summary>
        private bool _modified = false;

        /// <summary>
        /// Default constructor for the experiences page.
        /// </summary>
        public ProjectExperiencesPage() {

            // Render the page
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

        }

        /// <summary>
        /// Method that is called when the page first appears.
        /// </summary>
        protected override async void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            // Check if the user is not in the first level of the program
            if (AppSettings.HorseProgram.Level != 
                AppSettings.HorseProgram.ProgramLevel.First) {

                // Disable the "other experiences" fields
                this.OtherExperiencesGrid.IsVisible = false;
                this.OtherExperiencesSeparator.IsVisible = false;

            }

            // Load the current active horse and bind it to the page.
            this._horse = HorseManager.GetInstance().ActiveHorse;
            int year = (DateTime.Now.Month < 7) ?
                DateTime.Now.Year - 1 : DateTime.Now.Year;

            // Attempt to get the existing set of project experiences for the
            // current horse at the current program level
            List<ProjectExperiences> experiences = await
                AppDatabase.GetInstance().Query<ProjectExperiences>(
                "SELECT * FROM ProjectExperiences WHERE HorseId = ? AND Level = ? AND Year = ?",
                this._horse.Id, AppSettings.HorseProgram.Level, year);

            // Check if no experiences exist
            if (experiences.Count == 0) {

                // Create a new ProjectExperiences collection and add it to the database
                this._experiences = new ProjectExperiences() {
                    HorseId = this._horse.Id,
                    Level = AppSettings.HorseProgram.Level,
                    Year = (DateTime.Now.Month < 7) ?
                        DateTime.Now.Year - 1 : DateTime.Now.Year,
                };
                await AppDatabase.GetInstance().Save<ProjectExperiences>(this._experiences);

            // Handle if the experiences already existed
            } else {
                this._experiences = experiences[0];
            }

            // Populate the editors on the page.
            this.FeedCareEditor.Text = this._experiences.FeedCareExperiences;
            this.HealthEditor.Text = this._experiences.HealthExperiences;
            this.LearningEditor.Text = this._experiences.LearningExperiences;
            this.GoalsEditor.Text = this._experiences.GoalsExperiences;
            this.OtherEditor.Text = this._experiences.OtherExperiences;

            // Set the "modified" property of the page to false
            this._modified = false;

        }

        /// <summary>
        /// Method that is called when the save button is clicked.
        /// </summary>
        /// <param name="sender">Object that sent the click event.</param>
        /// <param name="e">Event arguments for the  click event.</param>
        private async void SaveButton_Clicked(object sender,
            System.EventArgs e) {

            // Save the text in the editors
            this._experiences.FeedCareExperiences = this.FeedCareEditor.Text;
            this._experiences.HealthExperiences = this.HealthEditor.Text;
            this._experiences.LearningExperiences = this.LearningEditor.Text;
            this._experiences.GoalsExperiences = this.GoalsEditor.Text;
            this._experiences.OtherExperiences = this.OtherEditor.Text;

            // Get all of the content of the page and save it to the project
            // experiences database element
            await AppDatabase.GetInstance().Save<ProjectExperiences>(this._experiences);

            // Navigate to the previous page after the save operation
            await this.Navigation.PopAsync(true);

        }

        /// <summary>
        /// Method that is called when the cancel button is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Event arguments for the event.</param>
        private void CancelButton_Clicked(object sender, System.EventArgs e) {

            Device.BeginInvokeOnMainThread(async () => {

                // Check if any changes have been made to the project plan page
                if (!this._modified) {
                    await Navigation.PopAsync(true);
                    return;
                }

                // Display a confirmation dialog asking whether the user actually
                // wants to discard the modified information
                bool result = await this.DisplayAlert(
                    "Confirmation",
                    "Are you sure you want to discard any changes made to this page?",
                    "Discard",
                    "Cancel");

                // If the user selected cancel, exit from the lambda
                if (!result) return;

                // Navigate back without saving the horse
                await Navigation.PopAsync(true);

            });

        }

        /// <summary>
        /// Method that is called when the text is changed in an editor.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void Editor_TextChanged(object sender,
            TextChangedEventArgs e) {

            // Set the "modified" property of the page to true
            this._modified = true;

        }

    }

}