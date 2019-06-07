//
// _4H_Application.Models.Records.LaborRecordEntry.cs: Class that represents a
//   single entry in a user's labor record.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

using System;

namespace _4H_Application.Models.Records {

    /// <summary>
    /// Class that represents a single user entry in a user's labor record.
    /// </summary>
    public class LaborRecordEntry :
        RecordBookEntry {

        /// <summary>
        /// Reference to the labor start time.
        /// </summary>
        public TimeSpan Start {
            get; set;
        } = TimeSpan.Zero;

        /// <summary>
        /// Reference to the labor end time.
        /// </summary>
        public TimeSpan End {
            get; set;
        } = TimeSpan.Zero;

        /// <summary>
        /// Reference to the labor description.
        /// </summary>
        public string Description {
            get; set;
        } = string.Empty;

    }

}
