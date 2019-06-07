//
// _4H_Application.Views.Pages.Project.ProjectRequirementsPage.xaml.cs: Page
//   that serves as a view for the project requirements based on their project
//   level.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause (jkjk8080@gmail.com)
//

using _4H_Application.Models.Projects;
using _4H_Application.Models.Settings;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Projects {

    /// <summary>
    /// Class that shows all of the project requirements.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectRequirementsPage : ContentPage {

        /// <summary>
        /// Default constructor for the requirements page.
        /// </summary>
        public ProjectRequirementsPage() {
            InitializeComponent();
        }

        /// <summary>
        /// Method that is called when the page first appears.
        /// </summary>
        protected override void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            // Get the horse program level of the user
            List<string> requirements = null;
            if ((int) AppSettings.HorseProgram.Level == 1) {
                requirements = ProjectRequirement.Requirements1;
            } else {
                requirements = ProjectRequirement.Requirements2;
            }

            // Go through each requirement
            for (int i = 0; i < requirements.Count; i ++) {

                // Create the grid and define all of the spacing properties
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition {
                    Width = 25 });
                grid.ColumnDefinitions.Add(new ColumnDefinition {
                    Width = GridLength.Star });
                grid.RowDefinitions.Add(new RowDefinition {
                    Height = GridLength.Star });
                grid.ColumnSpacing = 5f;
                grid.Margin = new Thickness {
                    Top = 10,
                    Bottom = 10,
                    Left = 12,
                    Right = 12,
                };

                // Add the number label to the left side of the grid
                grid.Children.Add(new Label {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                    FontSize = 14f,
                    FontAttributes = FontAttributes.Bold,
                    Text = (i + 1).ToString(),
                }, 0, 0);

                // Add the content label to the right side of the grid
                grid.Children.Add(new Label {
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Start,
                    FontSize = 14f,
                    Text = requirements[i],
                    AutomationId = "Requirement_" + i
                }, 1, 0);

                // Create the ViewCell to put everything inside, then add it
                ViewCell cell = new ViewCell {
                    View = grid,
                };
                this.RequirementsList.Add(cell);

            }

        }

    }

}