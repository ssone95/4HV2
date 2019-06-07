//
// _4H_Application.Models.Exports.IExportHelper.cs: Interface to provide 
//   platform-specific export-relevant features.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

namespace _4H_Application.Models.Exports {

    /// <summary>
    /// Interface to provide export-relevant features.
    /// </summary>
    public interface IExportHelper {

        string GetDocumentsDirectory();

        /// <summary>
        /// Method to get the default system font.
        /// </summary>
        /// <returns>The default system font.</returns>
        string GetDefaultSystemFont();

        void SendEmail(string file);
    }

}
