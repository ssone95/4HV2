//
// _4H_Application.Views.Pages.Records.FeedRecordPage.xaml.cs: Page that
//   allows a user to manage all their feed for a specific date.
//
// Feed in the 4-H horse program can range anywhere from grain, hay, salt or
//   other minerals, pasture, or other feeds.
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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Records {

    /// <summary>
    /// Page that shows the user's feed record for a certain date.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedRecordPage : ContentPage {

        /// <summary>
        /// Reference to the maximum number of entries per day.
        /// </summary>
        public const int MAX_ENTRIES_PER_DAY = 5;

        /// <summary>
        /// Reference to the date associated with the feed record page.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// List of all the entries rendered on the page.
        /// </summary>
        private List<FeedRecordEntry> entries;

        /// <summary>
        /// Default constructor for the feed page.
        /// </summary>
        /// <param name="date">The date associated with the page.</param>
        public FeedRecordPage(DateTime date) {

            // Initialize the list of entries
            this.entries = new List<FeedRecordEntry>();

            // Initialize the default page
            this.date = date;
            InitializeComponent();

        }

        /// <summary>
        /// Method for when the page appears.
        /// </summary>
        protected override async void OnAppearing() {

            // Set the horse/date display at the page bottom
            this.SelectedDateLabel.Text =
                RecordBookTabbedPage.SelectedDate.ToString("MMM. dd, yyyy");
            this.SelectedHorseLabel.Text =
                HorseManager.GetInstance().ActiveHorse.Name;

            // Get all of the records for the page
            this.entries.Clear();
            this.entries = await RecordBookEntry.GetInRange
                <FeedRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id,
                                  this.date,
                                  this.date.AddDays(1));

            // Clear all of the contents from the activity record stack
            this.FeedRecordStack.Children.Clear();

            // Render all of the entries on the screen
            foreach (FeedRecordEntry entry in this.entries) {

                // Add a new record frame for each activity
                Device.BeginInvokeOnMainThread(() => {
                    FeedRecordView view = new FeedRecordView(entry);
                    this.FeedRecordStack.Children.Add(view);
                });

            }

        }

        /// <summary>
        /// Callback for when the Add Feed button is clicked.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the click event.</param>
        private async void AddFeedButton_Clicked(object sender, 
            EventArgs e) {

            // Check if the maximum number of entries for the day is
            // already reached
            if (this.entries.Count >= MAX_ENTRIES_PER_DAY) {
                ToastController.ShortToast("Cannot add any more records for the day!");
                return;
            }

            // Create a new record book entry and insert it into the
            // database to obtain a unique Id
            FeedRecordEntry entry = new FeedRecordEntry() {
                HorseId = HorseManager.GetInstance().ActiveHorse.Id,
                Date = date
            };
            await AppDatabase.GetInstance().Save<FeedRecordEntry>(entry);

            // Add a new record view to the stack
            this.entries.Add(entry);
            FeedRecordView view = new FeedRecordView(entry);
            this.FeedRecordStack.Children.Add(view);

        }

    }

}
