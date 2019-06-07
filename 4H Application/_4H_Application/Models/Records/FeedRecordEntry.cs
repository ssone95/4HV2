//
// _4H_Application.Models.Records.FeedRecordEntry.cs: Class that represents a
//   single entry in a user's feed record.
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
    /// Class that represents a single entry in a user's feed record.
    /// </summary>
    public class FeedRecordEntry : 
        RecordBookEntry {

        /// <summary>
        /// Reference to the feed type.
        /// </summary>
        public FeedType Type {
            get; set;
        } = FeedType.Grain;

        /// <summary>
        /// Reference to the feed description
        /// </summary>
        public string Description {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Reference to the feed amount, in pounds.
        /// </summary>
        public float Amount {
            get; set;
        } = 0.0f;

        /// <summary>
        /// Reference to the feed cost, in dollars.
        /// </summary>
        public float Cost {
            get; set;
        } = 0.0f;

    }
    
    /// <summary>
    /// List of available feed types.
    /// </summary>
    public enum FeedType {
        Hay,
        Grain,
        SaltAndMinerals,
        Pasture,
        Other,
    }

}
