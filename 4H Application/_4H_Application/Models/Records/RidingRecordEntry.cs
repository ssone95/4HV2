//
// _4H_Application.Models.Records.RidingRecordEntry.cs: Class that represents
//   a single entry in a user's riding record.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause (jkjk8080@gmail.com)
//

using System;

namespace _4H_Application.Models.Records {

    /// <summary>
    /// Class that represents a single entry in a user's riding record.
    /// </summary>
    public class RidingRecordEntry : 
        RecordBookEntry {

        /// <summary>
        /// Reference to the riding start time.
        /// </summary>
        public TimeSpan Start {
            get; set;
        } = TimeSpan.Zero;

        /// <summary>
        /// Reference to the riding end time.
        /// </summary>
        public TimeSpan End {
            get; set;
        } = TimeSpan.Zero;

        /// <summary>
        /// Reference to the riding description.
        /// </summary>
        public string Description {
            get; set;
        } = string.Empty;

    }

}
