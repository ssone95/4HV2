//
// _4H_Application.iOS.Data.FileHelper.cs: Implementation file for the iOS
//   architecture to assist with file handling.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.iOS.Data;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper))]
namespace _4H_Application.iOS.Data
{

    /// <summary>
    /// Class that assists with file handling on iOS devices.
    /// </summary>
    public class FileHelper : IFileHelper
    {

        /// <summary>
        /// Method to get a local file path.
        /// </summary>
        /// <param name="filename">The specified file name.</param>
        /// <returns>A string to the relevant file.</returns>
        public string GetLocalFilePath(string filename)
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var library = Path.Combine(documents, "..", "Library");

            // Create the directory if it does not yet exist
            if (!Directory.Exists(library))
            {
                Directory.CreateDirectory(library);
            }

            // return the combined filepath
            return Path.Combine(library, filename);

        }
        public string GenerateLocalAsset(string filename, string horsename)
        {

            /*string assetname = "Assets/" + filename;
            File.Copy(GetLocalFilePath(assetname), Path.Combine(Directory.GetCurrentDirectory(), "Assets/" + filename), true);
            return GetLocalFilePath(assetname);
            return assetname;*/
            return Path.Combine(Foundation.NSBundle.MainBundle.BundlePath, filename);
        }

    }

}