//
// _4H_Application.Android.Models.Exports.IExportHelper.cs: Implementation for
//   providing platform-specific export-relevant features.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.Droid.Models.Exports;
using _4H_Application.Models.Exports;
using Android.Content;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(ExportHelper))]
namespace _4H_Application.Droid.Models.Exports {

    /// <summary>
    /// Class to provide platform-specific export-relevant features.
    /// </summary>
    public class ExportHelper : IExportHelper {

        /// <summary>
        /// Method to get the default system font.
        /// </summary>
        /// <returns>The default system font.</returns>
        
        public string GetDefaultSystemFont() {
            return "sans-serif";
            //return "Lobster-Regular.ttf";
        }
        

        /// <summary>
        /// Method to get the documents directory.
        /// </summary>
        /// <returns>The device documents directory.</returns>
        public string GetDocumentsDirectory() {
            return Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments);
        }

        public void SendEmail(string file)
        {
            var f = new Java.IO.File(file);
            f.SetReadable(true, false);

            var email = new Intent(Intent.ActionSend);
            email.SetType("message/rfc822");
            email.PutExtra(Android.Content.Intent.ExtraSubject, "4-H Book");
            email.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(f));

            email.PutExtra(Intent.ExtraStream, Android.Support.V4.Content.FileProvider.GetUriForFile(Android.App.Application.Context, Android.App.Application.Context.PackageName + ".fileprovider", f));

            Forms.Context.StartActivity(Intent.CreateChooser(email, "Send email"));
        }
    }

}