//
// _4H_Application.Models.Records.ActivityRecordEntry.cs: Class that represents
//   a single entry in a user's activity record.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause (jkjk8080@gmail.com)
//

namespace _4H_Application.Models.Records {

    /// <summary>
    /// Class that represents a single entry in a user's activity record.
    /// </summary>
    public class ActivityRecordEntry : RecordBookEntry {

        /// <summary>
        /// Reference to the activity description.
        /// </summary>
        public string Description {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Reference to the activity location.
        /// </summary>
        public string Location {
            get; set;
        } = string.Empty;

    }

}
