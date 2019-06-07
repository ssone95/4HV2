//
// _4H_Application.Models.Records.RecordBookEntry.cs: Abstract class to serve
//   as a foundation for all record book entries.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause (jkjk8080@gmail.com)
//

using _4H_Application.Data;
using _4H_Application.Models.Horses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _4H_Application.Models.Records {

    /// <summary>
    /// Class that represents an entry in a record book.
    /// </summary>
    public abstract class RecordBookEntry : 
        DatabaseEntry {

        /// <summary>
        /// Reference to the Id of the horse that the record book entry is for.
        /// </summary>
        public int HorseId {
            get; set;
        } = -1;

        /// <summary>
        /// Reference to the date on which the record book entry occurred.
        /// </summary>
        public DateTime Date {
            get; set;
        } = DateTime.Now;

        /// <summary>
        /// Method to get a list of records for the given type in a range.
        /// </summary>
        /// <typeparam name="T">The type of record entry.</typeparam>
        /// <param name="start">The start date.</param>
        /// <param name="end">The end date.</param>
        /// <returns>A list of records in the specified range.</returns>
        public static async Task<List<T>> GetInRange<T>(
            int horseId, DateTime start, DateTime end) 
            where T : RecordBookEntry, new() {

            // Generate the SQL query to get the records
            string query = "SELECT * FROM " +
                typeof(T).Name + " WHERE " +
                "HorseId = ? AND Date >= ? AND Date < ?";

            // Return a promise with the specified records
            return await AppDatabase.GetInstance().Query<T>(
                query,
                horseId,
                start.Ticks,
                end.Ticks);

        }

    }

}
