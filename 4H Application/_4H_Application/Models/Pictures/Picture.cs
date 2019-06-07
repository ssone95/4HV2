//
// _4H_Application.Models.Pictures.Picture.cs: Class that represents a single
//   picture that has been saved to the user's record book.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;

namespace _4H_Application.Models.Pictures {

    /// <summary>
    /// Class that represents a single picture.
    /// </summary>
    public class Picture : DatabaseEntry {

        /// <summary>
        /// Default constructor for the picture object.
        /// </summary>
        public Picture() {
        }

        /// <summary>
        /// Reference to the Horse Id for the picture.
        /// </summary>
        public int HorseId {
            get; set;
        } = -1;

        /// <summary>
        /// Filepath to the specified picture.
        /// </summary>
        public string FilePath {
            get; set;
        } = string.Empty;

        /// <summary>
        /// The optional caption of the picture.
        /// </summary>
        public string Caption {
            get; set;
        } = string.Empty;

    }

}
