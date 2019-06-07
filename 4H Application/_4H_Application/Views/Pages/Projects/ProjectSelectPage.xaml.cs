//
// _4H_Application.Views.Pages.Project.ProjectPage.xaml.cs: Page that serves as
//   a gateway for the user to access all of their project content for a
//   particular date on their calendar.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Models.Horses;
using _4H_Application.Models.Settings;
using _4H_Application.Views.Components;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Projects {

    /// <summary>
    /// Page where a user can manager their project content.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectSelectPage : ContentPage {

        /// <summary>
        /// Reference to whether the page is locked or not
        /// </summary>
        private bool _locked {
            get; set;
        } = false;

        /// <summary>
        /// Default constructor for the project management page.
        /// </summary>
        public ProjectSelectPage() {
            InitializeComponent();
        }

        /// <summary>
        /// Method for when the page appears.
        /// </summary>
        protected override void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(() => {

                // Populate the current horse label
                this.RecordHorseLabel.Text =
                    (HorseManager.GetInstance().ActiveHorse == null) ?
                    "-" : HorseManager.GetInstance().ActiveHorse.Name;

                // Populate the record level label
                if (AppSettings.HorseProgram.Level == 
                    AppSettings.HorseProgram.ProgramLevel.Other) {
                    this.RecordLevelLabel.Text = "-";
                } else {
                    this.RecordLevelLabel.Text = string.Format(
                        "Level {0}", (int) AppSettings.HorseProgram.Level);
                }

            });

        }

        /// <summary>
        /// Method for when the project requirements frame is tapped.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the tap event.</param>
        private async void ProjectRequirements_Tapped(object sender, 
            System.EventArgs e) {

            // If the current page is set to be locked, do not render a new
            // page on top of it
            if (this._locked) {
                return;
            }

            // Lock the main thread from opening another page until the current
            // page has been closed.
            this._locked = true;

            // Push the page to the top of the navigation stack
            await this.Navigation.PushAsync(new ProjectRequirementsPage());

            // Unlock the page after the page has been created
            this._locked = false;

        }

        /// <summary>
        /// Method for when the project plan frame is tapped.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the tap event.</param>
        private async void ProjectPlan_Tapped(object sender, 
            System.EventArgs e) {

            // If the current page is set to be locked, do not render a new
            // page on top of it
            if (this._locked) {
                return;
            }

            // Check if there is no active horse
            if (HorseManager.GetInstance().ActiveHorse == null) {

                // If there is no active horse, send a toast to the screen to
                // tell the user to create a horse
                ToastController.ShortToast("Please create a horse first!");
                return;

            }

            // Lock the main thread from opening another page until the current
            // page has been closed.
            this._locked = true;

            // Push the page to the top of the navigation stack
            await this.Navigation.PushAsync(new ProjectPlanPage());

            // Unlock the page after the page has been created
            this._locked = false;

        }

        /// <summary>
        /// Method for when the project experiences frame is tapped.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the tap event.</param>
        private async void ProjectExperiences_Tapped(object sender,
            System.EventArgs e) {

            // If the current page is set to be locked, do not render a new
            // page on top of it
            if (this._locked) {
                return;
            }

            // Check if there is no active horse
            if (HorseManager.GetInstance().ActiveHorse == null) {

                // If there is no active horse, send a toast to the screen to
                // tell the user to create a horse
                ToastController.ShortToast("Please create a horse first!");
                return;

            }

            // Lock the main thread from opening another page until the current
            // page has been closed.
            this._locked = true;

            // Push the page to the top of the navigation stack
            await this.Navigation.PushAsync(new ProjectExperiencesPage());

            // Unlock the page after the page has been created
            this._locked = false;

        }

        /// <summary>
        /// Method for when the project story frame is tapped.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the tap event.</param>
        private async void ProjectStory_Tapped(object sender,
            System.EventArgs e) {

            // If the current page is set to be locked, do not render a new
            // page on top of it
            if (this._locked) {
                return;
            }

            // Check if there is no active horse
            if (HorseManager.GetInstance().ActiveHorse == null) {

                // If there is no active horse, send a toast to the screen to
                // tell the user to create a horse
                ToastController.ShortToast("Please create a horse first!");
                return;

            }

            // Lock the main thread from opening another page until the current
            // page has been closed.
            this._locked = true;

            // Push the page to the top of the navigation stack
            await this.Navigation.PushAsync(new ProjectStoryPage());

            // Unlock the page after the page has been created
            this._locked = false;

        }

    }

}