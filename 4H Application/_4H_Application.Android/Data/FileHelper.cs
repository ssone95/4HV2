//
// _4H_Application.Droid.Data.FileHelper.cs: Implementation file for the
//   Android architecture to assist with file handling.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Michael Okobi (mikaelobos@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Droid.Data;
using System;
using System.IO;
using Xamarin.Forms;
using Android.Content.Res;
using Android.App;
using _4H_Application.Models.Exports;
using System.Threading.Tasks;

[assembly: Dependency(typeof(FileHelper))]
namespace _4H_Application.Droid.Data
{

    /// <summary>
    /// Class that assists with file handling on Android devices.
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

            // Get the path to the personal folder and then combine it with the
            // specified filename
            string path = Environment.GetFolderPath(
                Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);

        }
        public string GenerateLocalAsset(string filename, string horsename)
        {
            /*string assetname = "Assets/" + filename;
            File.Copy(GetLocalFilePath(assetname),Path.Combine(Directory.GetCurrentDirectory(), "Assets/" + filename), true);
            return GetLocalFilePath(assetname);
            return assetname;*/
            var localFolder = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var filePath = Path.Combine(localFolder, horsename + filename);
            var assets = Android.App.Application.Context.Assets;

            using (var sr = new StreamReader(assets.Open(filename)))
            {
                using (var ms = new MemoryStream())
                {
                    sr.BaseStream.CopyTo(ms);
                    var bytes = ms.ToArray();
                    File.WriteAllBytes(filePath, bytes);
                }
            }
            //File.Copy(GetLocalFilePath("Assets/" + filename), Path.Combine(Directory.GetCurrentDirectory(), filename), true);
            //return "Assets/" + filename;
            return filePath;
        }

    }

}