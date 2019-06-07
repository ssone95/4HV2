//
// _4H_Application.Models.Records.BeddingRecordEntry.cs: Class that represents
//   a single entry in a user's bedding record.
//
// Author(s):
//   Brian Ristau (13ristaub@gmail.com)
//

namespace _4H_Application.Models.Records {

    /// <summary>
    /// Class that represents a single entry in a user's bedding record.
    /// </summary>
    public class BeddingRecordEntry : 
        RecordBookEntry {
        
        /// <summary>
        /// Reference to the type of bedding.
        /// </summary>
        public BeddingType Type {
            get; set;
        } = BeddingType.Straw;

        /// <summary>
        /// Reference to the amount (lbs) of bedding.
        /// </summary>
        public float Amount {
            get; set;
        } = 0.0f;

        /// <summary>
        /// Reference to the cost ($) of bedding.
        /// </summary>
        public float Cost {
            get; set;
        } = 0.0f;

    }

    /// <summary>
    /// List of available bedding types.
    /// </summary>
    public enum BeddingType {
        Hay,
        Hemp,
        Moss,
        Paper,
        Sawdust,
        Shavings,
        StallMats,
        Straw,
        WoodPellets,
    }

}
