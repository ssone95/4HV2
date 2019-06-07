//
// _4H_Application.Data.IFileHelper.cs: Interface that assists with a
//   platform-independent method of obtaining a specific file path.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Michael Okobi (mikaelobos@gmail.com)
//

namespace _4H_Application.Data
{

    /// <summary>
    /// Interface that assists obtaining a platform-independed file path.
    /// </summary>
    public interface IFileHelper
    {

        /// <summary>
        /// Method to get a local file path.
        /// </summary>
        /// <param name="filename">The specified file name.</param>
        /// <returns>A string to the relevant file.</returns>
        string GetLocalFilePath(string filename);
        string GenerateLocalAsset(string filename, string horsename);
    }

}