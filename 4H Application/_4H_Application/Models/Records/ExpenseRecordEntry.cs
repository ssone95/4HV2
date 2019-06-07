//
// _4H_Application.Models.Records.ExpenseRecordEntry.cs: Class that represents
//   a single entry in a user's expense record.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//   Joel Krause (jkjk8080@gmail.com)
//

namespace _4H_Application.Models.Records {

    /// <summary>
    /// Class that represents a single entry in a user's expense record.
    /// </summary>
    public class ExpenseRecordEntry : 
        RecordBookEntry {

        /// <summary>
        /// Reference to the description.
        /// </summary>
        public string Description {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Referense to the cost, in dollars.
        /// </summary>
        public float Cost {
            get; set;
        } = 0.0f;

    }

}
