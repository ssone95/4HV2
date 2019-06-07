//
// _4H_Application.Views.Pages.Records.CalendarPage.xaml.cs: Component page
//   that displays the calendar for the record book.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause (jkjk8080@gmail.com)
//

using _4H_Application.Models.Horses;
using _4H_Application.Views.Components.Records;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Records {

    /// <summary>
    /// Class that represents the calendar page for the record book.
    /// </summary>
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CalendarPage : ContentPage {

        /// <summary>
        /// Reference to the month/year that is displayed on the calendar.
        /// </summary>
        private DateTime DisplayedDate {
            get; set;
        } = DateTime.Today;

        /// <summary>
        /// Default constructor for the calendar page.
        /// </summary>
		public CalendarPage () {

            // Get the selected date from the tabbed page
            this.DisplayedDate = RecordBookTabbedPage.SelectedDate;

            // Render the calendar
            this.InitializeComponent();

		}

        /// <summary>
        /// Method that is called when the calendar appears.
        /// </summary>
        protected override void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            // Start a task to call the calendar update so that it does not
            // directly interfere with the rest of the calendar rendering
            Task.Run(() => {
                this.UpdateControls();
                this.UpdateCalendar();
            });

        }

        /// <summary>
        /// Method that will update the controls on the page.
        /// </summary>
        private void UpdateControls() {

            Device.BeginInvokeOnMainThread(() => {

                // Populate the selected Date/Horse labels
                this.SelectedDateLabel.Text =
                    RecordBookTabbedPage.SelectedDate.ToString("MMM. dd, yyyy");
                this.SelectedHorseLabel.Text =
                    (HorseManager.GetInstance().ActiveHorse == null) ?
                    "-" : HorseManager.GetInstance().ActiveHorse.Name;

                // Update the name display for the current month
                DateTime date = new DateTime(
                    this.DisplayedDate.Year,
                    this.DisplayedDate.Month,
                    1);
                this.MonthNameLabel.Text =
                    date.ToString("MMMM yyyy");

                // Update the name display for the navigation buttons
                this.MonthBackButton.Text =
                    date.AddMonths(-1).ToString("< MMM");
                this.MonthForwardButton.Text =
                    date.AddMonths(1).ToString("MMM >");

            });

        }

        /// <summary>
        /// Method that re-renders the calendar.
        /// </summary>
        private void UpdateCalendar() {

            // Call all updates on the main rendering thread
            Device.BeginInvokeOnMainThread(() => {

                // Update the name display for the current month
                DateTime date = new DateTime(
                    this.DisplayedDate.Year,
                    this.DisplayedDate.Month,
                    1);
                
                // Disable all the cells before the first day of the month
                int dayOfWeek = (int) date.DayOfWeek;
                for (int i = 0; i < dayOfWeek; i ++) {
                    ((CalendarCell) this.CalendarCellGrid.Children[i])
                        .Active = false;
                }

                // Disable all the cells after the last day of the month
                int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                for (int i = daysInMonth; i < 42; i ++) {
                    ((CalendarCell) this.CalendarCellGrid.Children[i])
                        .Active = false;
                }

                // Set the date and active property on the cells that belong in
                // the calendar
                for (int i = dayOfWeek; i < daysInMonth + dayOfWeek; i ++) {
                    ((CalendarCell) this.CalendarCellGrid.Children[i])
                        .Active = true;
                    ((CalendarCell)this.CalendarCellGrid.Children[i])
                        .Date = new DateTime(date.Year, 
                                             date.Month, 
                                             i - dayOfWeek + 1);
                }

                // Execute the grid update as a parallel operation
                Parallel.For(0, this.CalendarCellGrid.Children.Count,
                    new ParallelOptions {
                        MaxDegreeOfParallelism =
                        Environment.ProcessorCount
                    },
                    (idx) => {
                        ((CalendarCell) this.CalendarCellGrid.Children[idx])
                            .Update();
                    }
                );

            });

        }

        /// <summary>
        /// Method that is called when the month back button is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void MonthBackButton_Clicked(object sender, EventArgs e) {

            // Subtract a month to the current display and then re-render
            this.DisplayedDate = this.DisplayedDate.AddMonths(-1);
            this.UpdateControls();
            this.UpdateCalendar();

        }

        /// <summary>
        /// Method that is called when the month back button is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the click event.</param>
        /// <param name="e">Event arguments for the click event.</param>
        private void MonthForwardButton_Clicked(object sender, EventArgs e) {

            // Add a month to the current display and then re-render
            this.DisplayedDate = this.DisplayedDate.AddMonths(1);
            this.UpdateControls();
            this.UpdateCalendar();

        }

    }

}