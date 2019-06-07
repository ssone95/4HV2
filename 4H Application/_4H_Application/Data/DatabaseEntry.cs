//
// _4H_Application.Data.IDatabaseEntry.cs: Marker abstract class to flag the 
//   class as able to be saved in the 4H device database.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using SQLite;
using System;

namespace _4H_Application.Data {

    /// <summary>
    /// Abstract class that indicates that the specified class is able to be
    /// saved in the 4H device database.
    /// </summary>
    public abstract class DatabaseEntry {

        /// <summary>
        /// Unique Id for the specified entry.
        /// </summary>
        [PrimaryKey, AutoIncrement, Unique]
        public int Id {
            get; set;
        } = -1;

        /// <summary>
        /// Reference to when the entry was created.
        /// </summary>
        public DateTime CreationDate {
            get; set;
        } = DateTime.Now;

    }

}