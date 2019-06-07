﻿//
// _4H_Application.Views.Pages.Records.ServiceRecordPage.xaml.cs: Page that
//   allows a user to manage all their services for a specific date.
//
// Services in the 4-H horse program are various things such as riding lessons
//   or lessons, medical services such as de-worming, or general care services
//   such as farrier.
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
    /// Page that shows the user's services for a certain date.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceRecordPage : ContentPage {

        /// <summary>
        /// Reference to the maximum number of entries per day.
        /// </summary>
        public const int MAX_ENTRIES_PER_DAY = 5;

        /// <summary>
        /// Reference to the date associated with the services page.
        /// </summary>
        private DateTime date;

        /// <summary>
        /// List of all the entries rendered on the page.
        /// </summary>
        private List<ServiceRecordEntry> entries;

        /// <summary>
        /// Default constructor for the service page.
        /// </summary>
        /// <param name="date">The date associated with the page.</param>
        public ServiceRecordPage(DateTime date) {

            // Initialize the list of entries
            this.entries = new List<ServiceRecordEntry>();

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
                <ServiceRecordEntry>(HorseManager.GetInstance().ActiveHorse.Id,
                                      this.date,
                                      this.date.AddDays(1));

            // Clear all of the contents from the service record stack
            this.ServiceRecordStack.Children.Clear();

            // Render all of the entries on the screen
            foreach (ServiceRecordEntry entry in this.entries) {

                // Add a new record frame for each service
                Device.BeginInvokeOnMainThread(() => {
                    ServiceRecordView view = new ServiceRecordView(entry);
                    this.ServiceRecordStack.Children.Add(view);
                });

            }

        }

        /// <summary>
        /// Callback for when the Add Service button is clicked.
        /// </summary>
        /// <param name="sender">Object that triggered the event.</param>
        /// <param name="e">Arguments for the click event.</param>
        private async void AddServiceButton_Clicked(object sender,
            EventArgs e) {

            // Check if the maximum number of entries for the day is
            // already reached
            if (this.entries.Count >= MAX_ENTRIES_PER_DAY) {
                ToastController.ShortToast("Cannot add any more records for the day!");
                return;
            }

            // Create a new record book entry and insert it into the
            // database to obtain a unique Id
            ServiceRecordEntry entry = new ServiceRecordEntry() {
                HorseId = HorseManager.GetInstance().ActiveHorse.Id,
                Date = date,
            };
            await AppDatabase.GetInstance().Save<ServiceRecordEntry>(entry);

            // Add a new record view to the stack
            this.entries.Add(entry);
            ServiceRecordView view = new ServiceRecordView(entry);
            this.ServiceRecordStack.Children.Add(view);

        }

    }

}