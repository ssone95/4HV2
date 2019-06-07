//
// _4H_Application.Models.Pictures.PictureManager.cs: Class that represents a
//   singleton entity that manages all of the user's pictures.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace _4H_Application.Models.Pictures {

    /// <summary>
    /// Class that manages all of the record book pictures.
    /// </summary>
    public class PictureManager {

        /// <summary>
        /// Reference to the singleton PictureManager entity.
        /// </summary>
        private static PictureManager _instance = null;

        /// <summary>
        /// Public-accessible field for the PictureManager entity.
        /// </summary>
        public static PictureManager Instance {
            get {

                // Check if the instance is not yet instantiated
                if (PictureManager._instance == null) {
                    PictureManager._instance = new PictureManager();
                }

                // Return the singleton instance
                return PictureManager._instance;

            }
        }

        /// <summary>
        /// Internal list for all available pictures.
        /// </summary>
        private List<Picture> _Pictures = null;

        /// <summary>
        /// External reference to the picture list.
        /// </summary>
        public List<Picture> Pictures {
            get {
                return this._Pictures;
            }
        }

        /// <summary>
        /// Private constructor for the PictureManager class.
        /// </summary>
        private PictureManager() {

            // Initialize the internal list of pictures with all pictures that
            // are stored in the database.
            this._Pictures = new List<Picture>();
            Task.Run(async () => {
                await this.LoadPictures();
            });

        }

        /// <summary>
        /// Asynchronous method to reload the pictures.
        /// </summary>
        /// <returns></returns>
        public async Task LoadPictures() {
            this._Pictures = await 
                AppDatabase.GetInstance().GetAll<Picture>();
        }

        /// <summary>
        /// Method that will save/update a picture to the database.
        /// </summary>
        /// <param name="picture">The picture to save/update.</param>
        public async Task Save(Picture picture) {

            // Ensure that the picture is not null
            if (picture == null) {
                throw new NullReferenceException();
            }

            // Check if the picture has any incomplete fields
            // Each picture must have a filepath that is non-null.  In the
            // instance that a "Picture" references a non-existing file, it
            // will be simply cleaned from the database as an entry.
            if (picture.FilePath == string.Empty) {
                throw new Exception("Malformed filepath");
            }

            // Add the picture to the database - the information will
            // be added to the object automatically
            await AppDatabase.GetInstance().Save<Picture>(picture);

        }

        /// <summary>
        /// Method that will remove a picture from the database.
        /// </summary>
        /// <param name="picture">The picture to remove.</param>
        public async Task Remove(Picture picture) {

            // Ensure that the picture is not null
            if (picture == null) {
                return;
            }

            // Check if the picture is not in the list
            if (!this._Pictures.Contains(picture)) {
                return;
            }

            // Remove the picture from the list of pictures that are currently
            // loaded, and then remove it from the database
            this._Pictures.Remove(picture);
            await AppDatabase.GetInstance().Delete<Picture>(picture);

        }

    }

}
