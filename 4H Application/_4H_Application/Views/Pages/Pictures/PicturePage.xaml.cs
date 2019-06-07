//
// _4H_Application.Views.Pages.Pictures.PicturePage.xaml.cs: Class that
//   represents a page to manage all of the user's images for a single horse.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Models.Horses;
using _4H_Application.Models.Pictures;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _4H_Application.Views.Pages.Pictures {

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PicturePage : ContentPage {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PicturePage() {
            
            // Render the page
            InitializeComponent();

        }

        /// <summary>
        /// Method that is called when the page appears.
        /// </summary>
        protected override void OnAppearing() {

            // Invoke the base OnAppearing method
            base.OnAppearing();

            // For whatever reason, the page will continuously throw a null
            // reference exception after a photo is deleted.  Setting the
            // listview item source to null off of the device main thread seems
            // to fix this issue ... so please don't remove this line!
            this.PhotoListView.ItemsSource = null;

            // Refresh the list of pictures
            this.RefreshPictures();

        }

        /// <summary>
        /// Asynchronous task to refresh the picture ListView.
        /// </summary>
        private void RefreshPictures() {

            Device.BeginInvokeOnMainThread(async () => {

                // Begin the image list update
                this.PhotoListView.BeginRefresh();

                // Remove the contents of the Listview
                this.PhotoListView.ItemsSource = null;

                // Re-set the item source of the item list
                await PictureManager.Instance.LoadPictures();
                this.PhotoListView.ItemsSource = 
                    PictureManager.Instance.Pictures;
                
                // End the image list update after a short delay to make sure
                // that all of the contents have been loaded and layouts have
                // been computed fully
                await Task.Delay(750);
                this.PhotoListView.EndRefresh();

            });

        }

        /// <summary>
        /// Method that is called when "take picture" is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the click event.</param>
        private async void TakePictureButton_Clicked(object sender, 
            System.EventArgs e) {

            // Ensure that the device has a camera
            if (!CrossMedia.Current.IsCameraAvailable) {
                await DisplayAlert("No Camera", 
                                   "No camera available.", 
                                   "OK");
                return;
            }

            // Ensure that the device has the ability to take a picture
            if (!CrossMedia.Current.IsTakePhotoSupported) {
                await DisplayAlert("Error", 
                                   "Taking photos is not supported on this device", 
                                   "OK");
                return;
            }

            // Ensure there is an active horse selected
            if (HorseManager.GetInstance().ActiveHorse == null) {
                await DisplayAlert("Error",
                                   "A horse must be created before selecting any pictures",
                                   "OK");
                return;
            }

            // Take a picture using the device camera
            MediaFile file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions() {
                    Name = string.Format("4h-{0}.jpg", DateTime.Now.Ticks),
                    AllowCropping = true,
                    RotateImage = false,
                    SaveMetaData = true,
                    SaveToAlbum = true,
                }
            );

            // Check if the picture operation was cancelled
            if (file == null) {
                return;
            }

            // Save the picture to the list of pictures
            Picture picture = new Picture() {
                HorseId = HorseManager.GetInstance().ActiveHorse.Id,
                CreationDate = DateTime.Now,
                FilePath = file.Path,
            };

            // Save the picture to the database after it has been created
            await PictureManager.Instance.Save(picture);
            
            // Refresh the photo ListView
            this.RefreshPictures();

        }

        /// <summary>
        /// Method that is called when "select picture" is pressed.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the click event.</param>
        private async void SelectPictureButton_Clicked(object sender, 
            System.EventArgs e) {

            // Ensure that the device has the ability to select a picture
            if (!CrossMedia.Current.IsPickPhotoSupported) {
                await DisplayAlert("Error",
                                   "Picking photos is not supported on this device",
                                   "OK");
                return;
            }

            // Ensure there is an active horse selected
            if (HorseManager.GetInstance().ActiveHorse == null) {
                await DisplayAlert("Error",
                                   "A horse must be created before selecting any pictures",
                                   "OK");
                return;
            }

            // Select a photo from the user's gallery
            MediaFile file = await CrossMedia.Current.PickPhotoAsync(
                new PickMediaOptions());

            // Check if the picking operation was cancelled
            if (file == null) {
                return;
            }

            // Save the picture to the list of pictures
            Picture picture = new Picture() {
                HorseId = HorseManager.GetInstance().ActiveHorse.Id,
                CreationDate = DateTime.Now,
                FilePath = file.Path,
            };



            // Save the picture to the database after it has been created
            await PictureManager.Instance.Save(picture);
            
            // Refresh the photo ListView
            // this.RefreshPictures();

        }

        /// <summary>
        /// Method that is called when a photo is tapped.
        /// </summary>
        /// <param name="sender">Object that sent the event.</param>
        /// <param name="e">Arguments for the tap event.</param>
        private async void PhotoListView_ItemTapped(object sender, 
            ItemTappedEventArgs e) {

            // Check if the item selected is null
            if (e.Item == null) {
                return;
            }

            // Cast the item to work with later
            Picture picture = e.Item as Picture;
            if (Device.RuntimePlatform == Device.Android) {
                this.PhotoListView.SelectedItem = null;
            }

            // Open a new page for the user to edit/delete the specified image.
            Page page = new PictureEditPage(picture);
            await Navigation.PushAsync(page, true);

        }

    }

}