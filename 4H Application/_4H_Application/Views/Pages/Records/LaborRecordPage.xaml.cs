//
// _4H_Application.Views.Pages.Records.LaborRecordPage.xaml.cs: Page that
//   allows a user to manage all their labor for a specific date.
//
// Labor in the 4-H horse program can be a variety of different things, such
//   as clearing stalls or cleaning different areas of a barn.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause (jkjk8080@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Horses;
using _4H_Application.Models.Records;
using _4H_Application.Views.Components;
using _4H_Application.Views.Components.Records;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Records {

    /// <summary>
    /// Page that shows the user's labor for a certain date.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LaborRecordPage : ContentPage {

        /// <summary>
        /// Reference to the maximum number of entries per day.
        /// </summary>
        public const int MAX_ENTRIES_PER_DAY = 5;

        /// <summary>
        /// Reference to the date associated with the labor page.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// List of all the entries rendered on the page.
        /// </summary>
        private List<LaborRecordEntry> entries;

        /// <summary>
        /// Default constructor for the labor page.
        /// </summary>
        /// <param name="date">The date associated with the page.</param>
        public LaborRecordPage(DateTime date) {

            // Initialize the list of entries
            this.entries = new List<LaborRecordEntry>();

            // Initialize the default page
            this.date = date;
            InitializeComponent();
        }

        /// <summary>
        /// Method for when the page appears.
        /// </summary>
        protected override async void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            // Set the horse/date display at the page bottom
            this.SelectedDateLabel.Text =
                RecordBookTabbedPage.SelectedDate.ToString("MMM. dd, yyyy");
            this.SelectedHorseLabel.Text =
                HorseManager.GetInstance().ActiveHorse.Name;

            // Get all of the records for the page
            this.entries.Clear();
            this.entries = await RecordBookEntry.GetInRange
                <LaborRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id,
                                      this.date,
                                      this.date.AddDays(1));

            // Clear all of the contents from the labor record stack
            this.LaborRecordStack.Children.Clear();

            // Render all of the entries on the screen
            foreach (LaborRecordEntry entry in this.entries) {

                // Add a new record frame for each labor
                Device.BeginInvokeOnMainThread(() => {
                    LaborRecordView view = new LaborRecordView(entry);
                    this.LaborRecordStack.Children.Add(view);
                });

            }

        }

        /// <summary>
        /// Callback for when the Add Labor button is clicked.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the click event.</param>
        private async void AddLaborButton_Clicked(object sender,
            EventArgs e) {

            // Check if the maximum number of entries for the day is
            // already reached
            if (this.entries.Count >= MAX_ENTRIES_PER_DAY) {
                ToastController.ShortToast("Cannot add any more records for the day!");
                return;
            }

            // Create a new record book entry and insert it into the
            // database to obtain a unique Id
            LaborRecordEntry entry = new LaborRecordEntry() {
                HorseId = HorseManager.GetInstance().ActiveHorse.Id,
                Date = date
            };
            await AppDatabase.GetInstance().Save<LaborRecordEntry>(entry);

            // Add a new record view to the stack
            this.entries.Add(entry);
            LaborRecordView view = new LaborRecordView(entry);
            this.LaborRecordStack.Children.Add(view);

        }

    }

}