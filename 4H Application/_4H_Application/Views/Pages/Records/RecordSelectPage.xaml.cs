//
// _4H_Application.Views.Pages.Records.RecordSelectPage.xaml.cs: Page that
//   serves as a gateway for the user to access all of their records for a
//   particular date on their calendar.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Models.Horses;
using _4H_Application.Models.Records;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Records {

    /// <summary>
    /// Page where a user can manage their daily activities.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordSelectPage : ContentPage {

        /// <summary>
        /// Reference to the date for the page
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Default constructor for the record select page.
        /// </summary>
        public RecordSelectPage() {
            InitializeComponent();
        }

        /// <summary>
        /// Method for when the page appears.
        /// </summary>
        protected override void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            // Set the date for the page
            this.date = RecordBookTabbedPage.SelectedDate;
            this.locked = false;

            Device.BeginInvokeOnMainThread(() => {

                // Populate the selected Date/Horse labels
                this.SelectedDateLabel.Text =
                    RecordBookTabbedPage.SelectedDate.ToString("MMM. dd, yyyy");
                this.SelectedHorseLabel.Text =
                    (HorseManager.GetInstance().ActiveHorse == null) ?
                    "-" : HorseManager.GetInstance().ActiveHorse.Name;

            });

            // Update the record count for each of the cells on a separate
            // thread so as to not bog down the UI
            Task.Run(async () => {
                await this.UpdateRecordCount();
            });

        }

        /// <summary>
        /// Method that will asynchronously update the count of records.
        /// </summary>
        private async Task UpdateRecordCount() {

            // Update the ActivityRecord display
            List<ActivityRecordEntry> activityList = await RecordBookEntry.GetInRange
                <ActivityRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id,
                                      this.date,
                                      this.date.AddDays(1));
            List<RidingRecordEntry> ridingList = await RecordBookEntry.GetInRange
                <RidingRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id, 
                                    this.date, 
                                    this.date.AddDays(1));
            List<FeedRecordEntry> feedList = await RecordBookEntry.GetInRange
                <FeedRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id,
                                  this.date,
                                  this.date.AddDays(1));
            List<BeddingRecordEntry> beddingList = await RecordBookEntry.GetInRange
                <BeddingRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id,
                                     this.date,
                                     this.date.AddDays(1));
            List<LaborRecordEntry> laborList = await RecordBookEntry.GetInRange
                <LaborRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id,
                                   this.date,
                                   this.date.AddDays(1));
            List<ServiceRecordEntry> serviceList = await RecordBookEntry.GetInRange
                <ServiceRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id,
                                     this.date,
                                     this.date.AddDays(1));
            List<ExpenseRecordEntry> expenseList = await RecordBookEntry.GetInRange
                <ExpenseRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id,
                                     this.date,
                                     this.date.AddDays(1));

            Device.BeginInvokeOnMainThread(() => {

                // Set the contents of the activity record pane
                this.ActivityCountLabel.Text = string.Format("{0} {1}", 
                    activityList.Count, 
                    activityList.Count == 1 ? "entry" : "entries");
                this.ActivityCountLabel.TextColor = (activityList.Count == 0) ?
                    Color.LightGray : Color.Default;

                // Set the contents of the riding record pane
                this.RidingCountLabel.Text = string.Format("{0} {1}",
                    ridingList.Count,
                    ridingList.Count == 1 ? "entry" : "entries");
                this.RidingCountLabel.TextColor = (ridingList.Count == 0) ?
                    Color.LightGray : Color.Default;

                // Set the contents of the feed record pane
                this.FeedCountLabel.Text = string.Format("{0} {1}",
                    feedList.Count,
                    feedList.Count == 1 ? "entry" : "entries");
                this.FeedCountLabel.TextColor = (feedList.Count == 0) ?
                    Color.LightGray : Color.Default;

                // Set the contents of the bedding record pane
                this.BeddingCountLabel.Text = string.Format("{0} {1}",
                    beddingList.Count,
                    beddingList.Count == 1 ? "entry" : "entries");
                this.BeddingCountLabel.TextColor = (beddingList.Count == 0) ?
                    Color.LightGray : Color.Default;

                // Set the contents of the labor record pane
                this.LaborCountLabel.Text = string.Format("{0} {1}",
                    laborList.Count,
                    laborList.Count == 1 ? "entry" : "entries");
                this.LaborCountLabel.TextColor = (laborList.Count == 0) ?
                    Color.LightGray : Color.Default;

                // Set the contents of the service record pane
                this.ServiceCountLabel.Text = string.Format("{0} {1}",
                    serviceList.Count,
                    serviceList.Count == 1 ? "entry" : "entries");
                this.ServiceCountLabel.TextColor = (serviceList.Count == 0) ?
                    Color.LightGray : Color.Default;

                // Set the contents of the expense record pane
                this.ExpenseCountLabel.Text = string.Format("{0} {1}",
                    expenseList.Count,
                    expenseList.Count == 1 ? "entry" : "entries");
                this.ExpenseCountLabel.TextColor = (expenseList.Count == 0) ?
                    Color.LightGray : Color.Default;

            });

        }

        /// <summary>
        /// Method that is called when the hardware back button is pressed.
        /// </summary>
        /// <returns></returns>
        protected override bool OnBackButtonPressed() {

            // Navigate to the calendar page and suppress the back request
            RecordBookTabbedPage.NavigateTo(
                RecordBookTabbedPage.TabbedPageOption.CalendarPage);
            return true;

        }

        /// <summary>
        /// Reference to the lock for navigating to a child page.
        /// </summary>
        private bool locked;

        /// <summary>
        /// Callback method for when the activity record cell is tapped.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private async void ActivityRecord_Tapped(object sender, EventArgs e) {

            // If the page is currently locked, exit
            if (this.locked) {
                return;
            }

            // Lock the page
            this.locked = true;

            // Open a new activity record page for the current date
            Page page = new ActivityRecordPage(this.date);
            await this.Navigation.PushAsync(page);

            // Unlock the page
            this.locked = false;

        }

        /// <summary>
        /// Callback method for when the riding record cell is tapped.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void RidingRecord_Tapped(object sender, EventArgs e) {

            // If the page is currently locked, exit
            if (this.locked) {
                return;
            }

            // Lock the page
            this.locked = true;

            // Open a new riding record page for the current date
            Page page = new RidingRecordPage(this.date);
            this.Navigation.PushAsync(page);

            // Unlock the page
            this.locked = false;

        }

        /// <summary>
        /// Callback method for when the feed record cell is tapped.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void FeedRecord_Tapped(object sender, EventArgs e) {

            // If the page is currently locked, exit
            if (this.locked) {
                return;
            }

            // Lock the page
            this.locked = true;

            // Open a new feed record page for the current date
            Page page = new FeedRecordPage(this.date);
            this.Navigation.PushAsync(page);

            // Unlock the page
            this.locked = false;

        }

        /// <summary>
        /// Callback method for when the bedding record cell is tapped.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void BeddingRecord_Tapped(object sender, EventArgs e) {

            // If the page is currently locked, exit
            if (this.locked) {
                return;
            }

            // Lock the page
            this.locked = true;

            // Open a new bedding record page for the current date
            Page page = new BeddingRecordPage(this.date);
            this.Navigation.PushAsync(page);

            // Unlock the page
            this.locked = false;

        }

        /// <summary>
        /// Callback method for when the labor record cell is tapped.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void LaborRecord_Tapped(object sender, EventArgs e) {

            // If the page is currently locked, exit
            if (this.locked) {
                return;
            }

            // Lock the page
            this.locked = true;

            // Open a new labor record page for the current date
            Page page = new LaborRecordPage(this.date);
            this.Navigation.PushAsync(page);

            // Unlock the page
            this.locked = false;

        }

        /// <summary>
        /// Callback method for when the service record cell is tapped.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void ServiceRecord_Tapped(object sender, EventArgs e) {

            // If the page is currently locked, exit
            if (this.locked) {
                return;
            }

            // Lock the page
            this.locked = true;

            // Open a new service record page for the current date
            Page page = new ServiceRecordPage(this.date);
            this.Navigation.PushAsync(page);

            // Unlock the page
            this.locked = false;

        }

        /// <summary>
        /// Callback method for when the expense record cell is tapped.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the event.</param>
        private void ExpenseRecord_Tapped(object sender, EventArgs e) {

            // If the page is currently locked, exit
            if (this.locked) {
                return;
            }

            // Lock the page
            this.locked = true;

            // Open a new feed record page for the current date
            Page page = new ExpenseRecordPage(this.date);
            this.Navigation.PushAsync(page);

            // Unlock the page
            this.locked = false;

        }

    }

}
