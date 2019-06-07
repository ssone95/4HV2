//
// _4H_Application.iOS.Models.Exports.IExportHelper.cs: Implementation for
//   providing platform-specific export-relevant features.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using _4H_Application.iOS.Models.Exports;
using _4H_Application.Models.Exports;
using MessageUI;
using System;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ExportHelper))]
namespace _4H_Application.iOS.Models.Exports {

    /// <summary>
    /// Class to provide platform-specific export-relevant features.
    /// </summary>
    public class ExportHelper : IExportHelper {

        /// <summary>
        /// Method to get the default system font.
        /// </summary>
        /// <returns>The default system font.</returns>
        ///
        
        public string GetDefaultSystemFont() {
            return "Helvetica";
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
            MFMailComposeViewController mailController;

            if (MFMailComposeViewController.CanSendMail)
            {
                mailController = new MFMailComposeViewController();
                //mailController.SetToRecipients(new string[] { "john@doe.com" });
                mailController.SetSubject("4-H PDF");

                // set attachment
                mailController.AddAttachmentData(Foundation.NSData.FromFile(file), @"application/pdf", "4-H PDF");


                mailController.Finished += (object s, MFComposeResultEventArgs args) => {
                    Console.WriteLine(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };


                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(mailController, true, null);
            }
        }

    }

}