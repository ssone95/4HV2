//
// _4H_Application.Views.Pages.Project.ProjectStoryPage.xaml.cs: Page that 
//   allows a user to manage their project story for the current horse.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Horses;
using _4H_Application.Models.Projects;
using _4H_Application.Views.Components;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Projects {

    /// <summary>
    /// Class that allows a user to manage their project story for a horse.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectStoryPage : ContentPage {

        /// <summary>
        /// Reference to the current horse.
        /// </summary>
        private Horse _horse = null;

        /// <summary>
        /// Reference to the current project story.
        /// </summary>
        private ProjectStory _story = null;

        /// <summary>
        /// Reference to whether the page has been changed.
        /// </summary>
        private bool _modified = false;

        /// <summary>
        /// Default constructor for the project story page.
        /// </summary>
        public ProjectStoryPage() {

            // Render the page
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

        }

        /// <summary>
        /// Method that is called when the page first appears.
        /// </summary>
        protected override async void OnAppearing() {

            // Call the base OnAppearing method
            base.OnAppearing();

            // Load the current active horse and bind it to the page
            this._horse = HorseManager.GetInstance().ActiveHorse;
            int year = (DateTime.Now.Month < 7) ?
                DateTime.Now.Year - 1 : DateTime.Now.Year;

            // Attempt to get the existing project story for the current horse
            List<ProjectStory> stories = await
                AppDatabase.GetInstance().Query<ProjectStory>(
                "SELECT * FROM ProjectStory WHERE HorseId = ? AND Year = ?",
                this._horse.Id, year);

            // Check if no story exists for the current horse
            if (stories.Count == 0) {

                // Create a new story and then add it to the database
                this._story = new ProjectStory() {
                    HorseId = this._horse.Id,
                    Year = (DateTime.Now.Month < 7) ?
                        DateTime.Now.Year - 1 : DateTime.Now.Year,
                };
                await AppDatabase.GetInstance().Save<ProjectStory>(this._story);

            // Set the plan if it already exists
            } else {
                this._story = stories[0];
            }

            // Populate the project story editor
            this.ProjectStoryEditor.Text = this._story.Story;
            this.ProjectStoryEditor.Keyboard = Keyboard.Create(
                KeyboardFlags.All);

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

            // Save the text in the editor
            this._story.Story = this.ProjectStoryEditor.Text;

            // Get all of the content of the page and save it to the project
            // story database element
            await AppDatabase.GetInstance().Save<ProjectStory>(this._story);

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
        private void FieldModified(object sender,
            TextChangedEventArgs e) {

            // Set the "modified" property of the page to true
            this._modified = true;

        }

    }

}